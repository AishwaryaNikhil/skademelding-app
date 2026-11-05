namespace TelenorSkademeldingApp.Application.DamageClaims.Dtos;

/// <summary>Input needed to register a new damage claim.</summary>
public sealed class CreateClaimRequest
{
    public int DamageTypeId { get; set; }

    public int DamageCauseId { get; set; }

    public string Summary { get; set; } = string.Empty;
}
