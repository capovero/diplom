using backendtest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace backendtest.Configurations;
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id); 
        builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).IsRequired().HasColumnType("text");
        builder.Property(p => p.GoalAmount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.CollectedAmount).HasColumnType("decimal(18,2)");
        builder.HasOne<User>(p => p.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.UserId);
        builder.HasOne<Category>(p => p.Category)
            .WithMany(c => c.Projects) 
            .HasForeignKey(p => p.CategoryId)
            .IsRequired(false);
        builder.HasMany<Update>(p => p.Updates)
            .WithOne(u => u.Project)
            .HasForeignKey(u => u.ProjectId);
        builder.HasMany<Media>(p => p.MediaFiles)
            .WithOne(m => m.Project)
            .HasForeignKey(m => m.ProjectId);
        builder.HasMany<Donation>(p => p.Donations)
            .WithOne(d => d.Project)
            .HasForeignKey(d => d.ProjectId);
        builder.Property(p => p.Status)
            .HasConversion<string>();
    }
}