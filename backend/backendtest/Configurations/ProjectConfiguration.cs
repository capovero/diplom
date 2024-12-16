using backendtest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendtest.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id); // Первичный ключ

        builder.Property(p => p.Title).IsRequired().HasMaxLength(100); // Название проекта
        builder.Property(p => p.Description).IsRequired().HasColumnType("text"); // Описание
        builder.Property(p => p.GoalAmount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.CollectedAmount).HasColumnType("decimal(18,2)");

        // Связь с пользователем (создателем проекта)
        builder.HasOne<User>(p => p.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.UserId);

        // Связь с категорией
        builder.HasOne<Category>(p => p.Category)
            .WithMany(c => c.Projects) // Привязываем к коллекции Projects в категории
            .HasForeignKey(p => p.CategoryId)
            .IsRequired(false);

        // Связь с обновлениями
        builder.HasMany<Update>(p => p.Updates)
            .WithOne(u => u.Project)
            .HasForeignKey(u => u.ProjectId);

        // Связь с вложениями (медиа)
        builder.HasMany<Media>(p => p.MediaFiles)
            .WithOne(m => m.Project)
            .HasForeignKey(m => m.ProjectId);

        // Связь с пожертвованиями
        builder.HasMany<Donation>(p => p.Donations)
            .WithOne(d => d.Project)
            .HasForeignKey(d => d.ProjectId);

        // Указываем хранение статуса как строкового значения
        builder.Property(p => p.Status)
            .HasConversion<string>(); // Сохранять статус как строку в базе данных
    }
}