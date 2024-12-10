using backendtest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendtest.Configurations;

public class UpdateConfiguration : IEntityTypeConfiguration<Update>
{
    public void Configure(EntityTypeBuilder<Update> builder)
    {
        builder.HasKey(u => u.Id); // Первичный ключ

        builder.Property(u => u.Content).IsRequired().HasColumnType("text"); // Обязательное поле

        // Связь с проектом
        builder.HasOne<Project>(u => u.Project)
            .WithMany(p => p.Updates)
            .HasForeignKey(u => u.ProjectId);
    }
}