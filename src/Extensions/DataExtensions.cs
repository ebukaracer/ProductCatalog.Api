using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Repositories;

namespace ProductCatalog.Api.Extensions;

public static class DataExtensions
{
    public static void AddRepositories(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        var isDevelopment = builder.Environment.IsDevelopment();

        // Use SQLite in Development environment for simplicity(without migrations).
        var key = isDevelopment ? "Database:ConnectionString:SQLiteDB" : "Database:ConnectionString:SQLServerDB";

        if (!isDevelopment && string.IsNullOrEmpty(builder.Configuration.GetValue<string>(key)))
            throw new Exception("SQL Server connection string is not configured.");

        var connStr = builder.Configuration.GetValue<string>(key)
                      ?? "Data Source=productcatalog.db";

        services.AddDbContext<OrderDbContext>(options =>
        {
            if (isDevelopment)
                options.UseSqlite(connStr);
            else
                options.UseSqlServer(connStr);
        });
    }
}