using Microsoft.EntityFrameworkCore;

namespace CrowdfundingPlatformBackend;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost; port=5432; database=registration;Username=postgres;Password=");
    }

}