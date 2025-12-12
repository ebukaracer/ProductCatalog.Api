using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Data;

/// <summary>
/// Populates the database with initial data if configured to do so(true by default).
/// Will also run migrations if configured to do so(true by default).
/// See appsettings.Development.json file.
/// </summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration config)
    {
        await using var dbContext = new OrderDbContext(serviceProvider
            .GetRequiredService<DbContextOptions<OrderDbContext>>());

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();


        var runMigration = config.GetValue<bool>("Database:RunMigration");
        var seedInitialData = config.GetValue<bool>("Database:SeedInitialData");

        // Apply migrations or create database; don't use migrations in development for faster iterations.
        if (serviceProvider
            .GetRequiredService<IHostEnvironment>()
            .IsDevelopment())
            runMigration = false;

        if (runMigration)
            await dbContext.Database.MigrateAsync();
        else
            await dbContext.Database.EnsureCreatedAsync();

        if (!dbContext.Products.Any() && seedInitialData)
        {
            logger.LogInformation("Seeding initial products...");

            dbContext.Products.AddRange(
                new Product
                {
                    Name = "Laptop", Description = "High-performance laptop", Price = 850_000, StockQty = 20
                },
                new Product
                {
                    Name = "Headset", Description = "Wireless gaming headset", Price = 45_000, StockQty = 50
                },
                new Product
                {
                    Name = "Keyboard", Description = "Mechanical keyboard", Price = 30_000, StockQty = 40
                },
                new Product
                {
                    Name = "Mouse", Description = "Ergonomic optical mouse", Price = 12_000, StockQty = 100
                },
                new Product
                {
                    Name = "Smartphone", Description = "Latest generation smartphone", Price = 400_000, StockQty = 15
                },
                new Product
                {
                    Name = "USB Cable", Description = "Durable USB Type-C cable", Price = 3_000, StockQty = 200
                }
            );

            await dbContext.SaveChangesAsync();
            logger.LogInformation("Products seeded.");
        }
        else
            logger.LogInformation("Some products already exist in DB. Skipping seeding...");
    }
}