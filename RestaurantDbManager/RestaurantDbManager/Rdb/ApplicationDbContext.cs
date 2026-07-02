using Microsoft.EntityFrameworkCore;
using RestaurantDbManager.Model;
using RestaurantDbManager.Rdb.Entities;

namespace RestaurantDbManager.Rdb;

public class ApplicationDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<MenuPositionImage> MenuPositionImages { get; set; }
    public DbSet<MenuPositionOrder> MenuPositionOrders { get; set; }
    public DbSet<MenuPosition> MenuPositions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        string useConnection = configuration.GetSection("UseConnection").Value ?? "DefaultConnection";
        string? connectionString = configuration.GetConnectionString(useConnection);
        optionsBuilder.UseSqlServer(connectionString);
    }
}