using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using TelenorSkademeldingApp.Api.Auth;

namespace TelenorSkademeldingApp.Api.Endpoints;

public static class AuthEndpoints
{
    /// <summary>
    /// Test login: exchanges the configured test credentials for a signed JWT.
    /// Only mapped in Development — replace with a real identity provider (e.g. Entra ID) for production.
    /// </summary>
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            return app;
        }

        app.MapPost("/api/auth/token", (
                LoginRequest request,
                IOptions<TestAuthOptions> testAuth,
                JwtTokenService tokens) =>
            {
                var expected = testAuth.Value;
                if (!SecureEquals(request.Username, expected.Username) ||
                    !SecureEquals(request.Password, expected.Password))
                {
                    return Results.Unauthorized();
                }

                var (token, expiresAtUtc) = tokens.CreateToken(expected.Username);
                return Results.Ok(new TokenResponse(token, "Bearer", expiresAtUtc));
            })
            .AllowAnonymous()
            .WithTags("Auth")
            .WithSummary("Get a JWT using the test credentials (Development only)");

        return app;
    }

    private static bool SecureEquals(string? actual, string expected) =>
        actual is not null &&
        CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(actual), Encoding.UTF8.GetBytes(expected));
}
