using backendtest.Models;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Data;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost; port=5432; database=registration;Username=postgres;Password=");
    }
    
}