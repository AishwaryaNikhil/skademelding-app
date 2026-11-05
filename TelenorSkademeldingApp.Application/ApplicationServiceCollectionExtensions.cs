using Microsoft.Extensions.DependencyInjection;
using TelenorSkademeldingApp.Application.Common.Interfaces;
using TelenorSkademeldingApp.Application.DamageClaims;

namespace TelenorSkademeldingApp.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IClaimService, ClaimService>();
        return services;
    }
}
