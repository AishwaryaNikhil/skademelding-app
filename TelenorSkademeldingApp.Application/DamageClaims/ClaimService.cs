using Microsoft.Extensions.Options;
using TelenorSkademeldingApp.Application.Common.Exceptions;
using TelenorSkademeldingApp.Application.Common.Interfaces;
using TelenorSkademeldingApp.Application.Common.Options;
using TelenorSkademeldingApp.Application.DamageClaims.Dtos;
using TelenorSkademeldingApp.Application.Models;
using TelenorSkademeldingApp.Domain.Entities;
using TelenorSkademeldingApp.Domain.Enums;

namespace TelenorSkademeldingApp.Application.DamageClaims;

public sealed class ClaimService : IClaimService
{
    private readonly IClaimRepository _claims;
    private readonly ILookupRepository _lookups;
    private readonly IFileStorage _storage;
    private readonly FileUploadOptions _uploadOptions;

    public ClaimService(
        IClaimRepository claims,
        ILookupRepository lookups,
        IFileStorage storage,
        IOptions<FileUploadOptions> uploadOptions)
    {
        _claims = claims;
        _lookups = lookups;
        _storage = storage;
        _uploadOptions = uploadOptions.Value;
    }

    public async Task<ClaimResponse> CreateClaimAsync(
        CreateClaimRequest request,
        IReadOnlyList<FileUpload> damageImages,
        IReadOnlyList<FileUpload> repairFiles,
        string? reportedBy,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Summary))
        {
            throw new ClaimValidationException("En beskrivelse av hendelsen er påkrevd.");
        }

        if (!await _lookups.DamageTypeExistsAsync(request.DamageTypeId, cancellationToken))
        {
            throw new ClaimValidationException($"Skadetype '{request.DamageTypeId}' finnes ikke.");
        }

        if (!await _lookups.DamageCauseExistsAsync(request.DamageCauseId, cancellationToken))
        {
            throw new ClaimValidationException($"Skadeårsak '{request.DamageCauseId}' finnes ikke.");
        }

        if (damageImages.Count > _uploadOptions.MaxFilesPerCategory ||
            repairFiles.Count > _uploadOptions.MaxFilesPerCategory)
        {
            throw new ClaimValidationException(
                $"Maks {_uploadOptions.MaxFilesPerCategory} filer er tillatt per kategori.");
        }

        foreach (var image in damageImages)
        {
            ValidateFile(image, _uploadOptions.AllowedImageContentTypes);
        }

        foreach (var file in repairFiles)
        {
            ValidateFile(file, _uploadOptions.AllowedRepairContentTypes);
        }

        var claimId = Guid.CreateVersion7();
        var claim = new Claim
        {
            Id = claimId,
            Reference = BuildReference(claimId),
            DamageTypeId = request.DamageTypeId,
            DamageCauseId = request.DamageCauseId,
            Summary = request.Summary.Trim(),
            Status = ClaimStatus.Submitted,
            ReportedBy = reportedBy
        };

        await AddAttachmentsAsync(claim, damageImages, AttachmentType.DamageSiteImage, cancellationToken);
        await AddAttachmentsAsync(claim, repairFiles, AttachmentType.RepairDocument, cancellationToken);

        await _claims.AddAsync(claim, cancellationToken);
        await _claims.SaveChangesAsync(cancellationToken);

        var created = await _claims.GetByIdAsync(claim.Id, cancellationToken);
        return MapToResponse(created!);
    }

    public async Task<ClaimResponse?> GetClaimAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var claim = await _claims.GetByIdAsync(id, cancellationToken);
        return claim is null ? null : MapToResponse(claim);
    }

    public async Task<IReadOnlyList<LookupItemResponse>> GetDamageTypesAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lookups.GetDamageTypesAsync(cancellationToken);
        return items.Select(x => new LookupItemResponse(x.Id, x.Code, x.Name, x.Description)).ToList();
    }

    public async Task<IReadOnlyList<LookupItemResponse>> GetDamageCausesAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lookups.GetDamageCausesAsync(cancellationToken);
        return items.Select(x => new LookupItemResponse(x.Id, x.Code, x.Name, x.Description)).ToList();
    }

    private async Task AddAttachmentsAsync(
        Claim claim,
        IReadOnlyList<FileUpload> files,
        AttachmentType type,
        CancellationToken cancellationToken)
    {
        foreach (var file in files)
        {
            var subPath = $"{claim.Id}/{type}";
            var stored = await _storage.SaveAsync(file, subPath, cancellationToken);

            claim.Attachments.Add(new ClaimAttachment
            {
                ClaimId = claim.Id,
                Type = type,
                FileName = file.FileName,
                ContentType = file.ContentType,
                StoragePath = stored.StoragePath,
                SizeBytes = stored.SizeBytes
            });
        }
    }

    private void ValidateFile(FileUpload file, string[] allowedContentTypes)
    {
        if (file.Length <= 0)
        {
            throw new ClaimValidationException($"Filen '{file.FileName}' er tom.");
        }

        if (file.Length > _uploadOptions.MaxFileSizeBytes)
        {
            throw new ClaimValidationException(
                $"Filen '{file.FileName}' er større enn maks tillatt størrelse ({_uploadOptions.MaxFileSizeBytes / (1024 * 1024)} MB).");
        }

        if (!allowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            throw new ClaimValidationException($"Filtypen '{file.ContentType}' er ikke tillatt for '{file.FileName}'.");
        }
    }

    // Human-friendly reference. The random tail of the UUID v7 is used (the leading bytes are a timestamp).
    private static string BuildReference(Guid id) =>
        $"CLM-{DateTime.UtcNow:yyyyMMdd}-{id.ToString("N")[^8..].ToUpperInvariant()}";

    private static ClaimResponse MapToResponse(Claim claim) => new(
        claim.Id,
        claim.Reference,
        claim.DamageTypeId,
        claim.DamageType?.Name ?? string.Empty,
        claim.DamageCauseId,
        claim.DamageCause?.Name ?? string.Empty,
        claim.Summary,
        claim.Status.ToString(),
        claim.ReportedBy,
        claim.CreatedAtUtc,
        claim.Attachments
            .Select(a => new ClaimAttachmentResponse(
                a.Id,
                a.Type.ToString(),
                a.FileName,
                a.ContentType,
                a.SizeBytes,
                a.CreatedAtUtc))
            .ToList());
}
