namespace TelenorSkademeldingApp.Api.Auth;

/// <summary>Test credentials for the demo login endpoint. Replace with a real identity provider.</summary>
public sealed class TestAuthOptions
{
    public const string SectionName = "TestAuth";

    public string Username { get; set; } = "test";

    public string Password { get; set; } = "test";
}
