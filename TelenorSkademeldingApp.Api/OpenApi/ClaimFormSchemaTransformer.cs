using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using TelenorSkademeldingApp.Api.Endpoints;

namespace TelenorSkademeldingApp.Api.OpenApi;

/// <summary>
/// Rewrites the multipart claim form schema so Swagger UI renders proper inputs:
/// integer ids, a summary text field and multi-file pickers for both attachment fields.
/// </summary>
internal sealed class ClaimFormSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type != typeof(CreateClaimForm))
        {
            return Task.CompletedTask;
        }

        schema.Properties = new Dictionary<string, IOpenApiSchema>
        {
            ["damageTypeId"] = new OpenApiSchema
            {
                Type = JsonSchemaType.Integer,
                Format = "int32",
                Description = "Type of damage. Id from GET /api/damage-types (e.g. 1 = car, 3 = area affected by wind)."
            },
            ["damageCauseId"] = new OpenApiSchema
            {
                Type = JsonSchemaType.Integer,
                Format = "int32",
                Description = "Cause of damage. Id from GET /api/damage-causes (e.g. 1 = accident, 2 = nature)."
            },
            ["summary"] = new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                MaxLength = 4000,
                Description = "Detailed summary of the incident."
            },
            ["damageImages"] = FileArray("Images of the damage site (skadested). JPEG/PNG/WEBP/GIF, max 10 MB each."),
            ["repairFiles"] = FileArray("Files after repair (etter reparasjon). Images or PDF, max 10 MB each.")
        };
        schema.Required = new HashSet<string> { "damageTypeId", "damageCauseId", "summary" };

        return Task.CompletedTask;
    }

    private static OpenApiSchema FileArray(string description) => new()
    {
        Type = JsonSchemaType.Array,
        Items = new OpenApiSchema { Type = JsonSchemaType.String, Format = "binary" },
        Description = description
    };
}
