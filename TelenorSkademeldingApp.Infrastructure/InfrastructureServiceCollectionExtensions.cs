using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelenorSkademeldingApp.Application.Common.Interfaces;
using TelenorSkademeldingApp.Infrastructure.Persistence;
using TelenorSkademeldingApp.Infrastructure.Repositories;
using TelenorSkademeldingApp.Infrastructure.Storage;

namespace TelenorSkademeldingApp.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? "Data Source=App_Data/telenor-skademelding.db";

        services.AddDbContext<SkademeldingDbContext>(options =>
            options.UseSqlite(connectionString));

        services.Configure<LocalFileStorageOptions>(
            configuration.GetSection(LocalFileStorageOptions.SectionName));

        services.AddScoped<IClaimRepository, ClaimRepository>();
        services.AddScoped<ILookupRepository, LookupRepository>();
        services.AddSingleton<IFileStorage, LocalFileStorage>();

        return services;
    }
}
