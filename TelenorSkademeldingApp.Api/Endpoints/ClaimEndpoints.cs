using System.Security.Claims;
using TelenorSkademeldingApp.Application.Common.Interfaces;
using TelenorSkademeldingApp.Application.DamageClaims.Dtos;
using TelenorSkademeldingApp.Application.Models;

namespace TelenorSkademeldingApp.Api.Endpoints;

public static class ClaimEndpoints
{
    private const string DamageImagesField = "damageImages";
    private const string RepairFilesField = "repairFiles";

    public static IEndpointRouteBuilder MapClaimEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api").RequireAuthorization();

        api.MapGet("/damage-types", (IClaimService service, CancellationToken ct) =>
                service.GetDamageTypesAsync(ct))
            .WithTags("Lookups");

        api.MapGet("/damage-causes", (IClaimService service, CancellationToken ct) =>
                service.GetDamageCausesAsync(ct))
            .WithTags("Lookups");

        var claims = api.MapGroup("/claims").WithTags("Claims");

        claims.MapPost("/", CreateClaimAsync)
            .Accepts<CreateClaimForm>("multipart/form-data")
            .Produces<ClaimResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithSummary("Report a new damage claim. Returns the generated claim id (UUID) and reference.");

        claims.MapGet("/{id:guid}", GetClaimAsync)
            .WithName("GetClaim")
            .Produces<ClaimResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get a claim by id (own claims only).");

        return app;
    }

    private static async Task<IResult> CreateClaimAsync(
        HttpRequest request,
        ClaimsPrincipal user,
        IClaimService service,
        CancellationToken ct)
    {
        if (!request.HasFormContentType)
        {
            return Results.Problem(
                "Forventet multipart/form-data.", statusCode: StatusCodes.Status415UnsupportedMediaType);
        }

        var form = await request.ReadFormAsync(ct);

        var errors = new Dictionary<string, string[]>();
        if (!int.TryParse(form["damageTypeId"], out var damageTypeId))
        {
            errors["damageTypeId"] = ["Skadetype er påkrevd."];
        }

        if (!int.TryParse(form["damageCauseId"], out var damageCauseId))
        {
            errors["damageCauseId"] = ["Skadeårsak er påkrevd."];
        }

        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        var createRequest = new CreateClaimRequest
        {
            DamageTypeId = damageTypeId,
            DamageCauseId = damageCauseId,
            Summary = form["summary"].ToString()
        };

        var opened = new List<Stream>();
        try
        {
            var damageImages = ToUploads(form.Files.GetFiles(DamageImagesField), opened);
            var repairFiles = ToUploads(form.Files.GetFiles(RepairFilesField), opened);

            var created = await service.CreateClaimAsync(
                createRequest, damageImages, repairFiles, user.Identity?.Name, ct);

            return Results.CreatedAtRoute("GetClaim", new { id = created.Id }, created);
        }
        finally
        {
            foreach (var stream in opened)
            {
                await stream.DisposeAsync();
            }
        }
    }

    private static async Task<IResult> GetClaimAsync(
        Guid id,
        ClaimsPrincipal user,
        IClaimService service,
        CancellationToken ct)
    {
        var claim = await service.GetClaimAsync(id, ct);

        // Users may only read claims they reported. 404 (not 403) so ids cannot be probed.
        if (claim is null || !string.Equals(claim.ReportedBy, user.Identity?.Name, StringComparison.Ordinal))
        {
            return Results.NotFound();
        }

        return Results.Ok(claim);
    }

    private static List<FileUpload> ToUploads(IReadOnlyList<IFormFile> files, List<Stream> opened) =>
        files.Select(f =>
        {
            var stream = f.OpenReadStream();
            opened.Add(stream);
            return new FileUpload
            {
                Content = stream,
                FileName = Path.GetFileName(f.FileName),
                ContentType = f.ContentType,
                Length = f.Length
            };
        }).ToList();
}
