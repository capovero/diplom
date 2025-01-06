using backendtest.Configurations;
using backendtest.Models;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Update> Updates { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Review> Reviews { get; set; } // Новая таблица

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UpdateConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new MediaConfiguration());
        modelBuilder.ApplyConfiguration(new DonationConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration()); // Настройки для новой таблицы
        base.OnModelCreating(modelBuilder);
    }
}