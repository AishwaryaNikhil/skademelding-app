namespace TelenorSkademeldingApp.Api.Endpoints;

/// <summary>JWT returned by POST /api/auth/token.</summary>
public sealed record TokenResponse(string AccessToken, string TokenType, DateTime ExpiresAtUtc);
