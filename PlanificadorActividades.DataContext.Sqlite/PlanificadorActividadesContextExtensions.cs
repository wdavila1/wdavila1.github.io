using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PlanificadorActividades.EntityModels;

public static class PlanificadorActividadesContextExtensions
{
    public static IServiceCollection AddPlanificadorActividadesContext(
        this IServiceCollection services,
        string relativePath = "..",
        string databaseName = "PlanificadorActividades.db")
    {
        string path = Path.Combine(relativePath, databaseName);
        path = Path.GetFullPath(path);
        PlanificadorActividadesLogger.WriteLine($"Ruta de la base de datos: {path}");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                message: $"{path} no encontrado.", fileName: path);
        }

        services.AddDbContext<PlanificadorActividadesContext>(options =>
        {
            options.UseSqlite($"Data Source={path}");
            options.LogTo(PlanificadorActividadesLogger.WriteLine,
                new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
        },
        contextLifetime: ServiceLifetime.Transient,
        optionsLifetime: ServiceLifetime.Transient);

        return services;
    }
}

