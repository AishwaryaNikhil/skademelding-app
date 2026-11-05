using TelenorSkademeldingApp.Application.DamageClaims.Dtos;
using TelenorSkademeldingApp.Application.Models;

namespace TelenorSkademeldingApp.Application.Common.Interfaces;

/// <summary>Use cases for reporting and retrieving damage claims.</summary>
public interface IClaimService
{
    Task<ClaimResponse> CreateClaimAsync(
        CreateClaimRequest request,
        IReadOnlyList<FileUpload> damageImages,
        IReadOnlyList<FileUpload> repairFiles,
        string? reportedBy,
        CancellationToken cancellationToken = default);

    Task<ClaimResponse?> GetClaimAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LookupItemResponse>> GetDamageTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LookupItemResponse>> GetDamageCausesAsync(CancellationToken cancellationToken = default);
}
