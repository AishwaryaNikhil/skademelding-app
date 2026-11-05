namespace TelenorSkademeldingApp.Api.Endpoints;

/// <summary>Shape of the multipart form for POST /api/claims; used for OpenAPI documentation only.</summary>
public sealed class CreateClaimForm
{
    public int DamageTypeId { get; set; }

    public int DamageCauseId { get; set; }

    public string Summary { get; set; } = string.Empty;

    public IFormFileCollection? DamageImages { get; set; }

    public IFormFileCollection? RepairFiles { get; set; }
}
