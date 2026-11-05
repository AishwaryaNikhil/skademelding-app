namespace TelenorSkademeldingApp.Api.Endpoints;

/// <summary>Body of POST /api/auth/token.</summary>
public sealed record LoginRequest(string Username, string Password);
