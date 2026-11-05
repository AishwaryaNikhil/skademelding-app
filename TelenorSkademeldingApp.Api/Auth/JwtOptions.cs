namespace TelenorSkademeldingApp.Api.Auth;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "TelenorSkademeldingApp";

    public string Audience { get; set; } = "TelenorSkademeldingApp";

    /// <summary>Symmetric signing key. Override via configuration/secret in real environments.</summary>
    public string Key { get; set; } = string.Empty;

    public int ExpiryMinutes { get; set; } = 60;
}
