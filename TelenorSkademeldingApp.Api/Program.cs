using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TelenorSkademeldingApp.Api.ErrorHandling;
using TelenorSkademeldingApp.Api.Auth;
using TelenorSkademeldingApp.Api.Endpoints;
using TelenorSkademeldingApp.Api.OpenApi;
using TelenorSkademeldingApp.Application;
using TelenorSkademeldingApp.Application.Common.Options;
using TelenorSkademeldingApp.Infrastructure;
using TelenorSkademeldingApp.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// --- Options ---
builder.Services.Configure<FileUploadOptions>(
    builder.Configuration.GetSection(FileUploadOptions.SectionName));
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<TestAuthOptions>(
    builder.Configuration.GetSection(TestAuthOptions.SectionName));

// --- Application & Infrastructure layers ---
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// --- Auth ---
builder.Services.AddSingleton<JwtTokenService>();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
if (Encoding.UTF8.GetByteCount(jwtOptions.Key) < 32)
{
    throw new InvalidOperationException(
        "Jwt:Key must be configured with at least 32 bytes (user secrets / environment variable / key vault).");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
        };
    });

builder.Services.AddAuthorization();

// --- Upload limits (several files of up to FileUpload:MaxFileSizeBytes each) ---
const long maxRequestBytes = 100L * 1024 * 1024;
builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = maxRequestBytes);
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o => o.MultipartBodyLengthLimit = maxRequestBytes);

// --- Cross-cutting ---
builder.Services.AddExceptionHandler<ClaimValidationExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Telenor Damage Claims API";
        document.Info.Description = "Report damage claims (skademelding) with site images and repair files.";
        return Task.CompletedTask;
    });
    options.AddDocumentTransformer<BearerSecurityTransformer>();
    options.AddOperationTransformer<BearerSecurityTransformer>();
    options.AddSchemaTransformer<ClaimFormSchemaTransformer>();
});

var app = builder.Build();

// Apply migrations on startup so the SQLite database and seed data are ready.
Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "App_Data"));
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SkademeldingDbContext>();
    db.Database.Migrate();
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Telenor Damage Claims API v1");
        options.RoutePrefix = "swagger";
        options.EnablePersistAuthorization();
    });
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints(app.Environment);
app.MapClaimEndpoints();

app.Run();
