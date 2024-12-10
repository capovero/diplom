using backendtest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendtest.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.HasKey(m => m.Id); // Первичный ключ

        builder.Property(m => m.FilePath).IsRequired().HasMaxLength(255); // Путь к файлу

        // Связь с проектом
        builder.HasOne<Project>(m => m.Project)
            .WithMany(p => p.MediaFiles)
            .HasForeignKey(m => m.ProjectId);
    }
}