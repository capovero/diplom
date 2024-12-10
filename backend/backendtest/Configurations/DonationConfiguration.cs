using backendtest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendtest.Configurations;

public class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.HasKey(d => d.Id); // Первичный ключ

        builder.Property(d => d.Amount).HasColumnType("decimal(18,2)"); // Сумма пожертвования

        // Связь с проектом
        builder.HasOne<Project>(d => d.Project)
            .WithMany(p => p.Donations)
            .HasForeignKey(d => d.ProjectId);

        // Связь с пользователем
        builder.HasOne<User>(d => d.User)
            .WithMany(u => u.Donations)
            .HasForeignKey(d => d.UserId);
    }
}