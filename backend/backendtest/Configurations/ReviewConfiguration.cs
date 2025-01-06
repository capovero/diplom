using backendtest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendtest.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id); // Устанавливаем первичный ключ
        builder.Property(r => r.Rating).IsRequired(); // Оценка обязательна
        builder.Property(r => r.Comment).HasMaxLength(1000); // Ограничение на длину комментария

        builder.HasOne(r => r.Project) // Связь с проектом
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProjectId)
            .OnDelete(DeleteBehavior.Cascade); // Удаление отзывов при удалении проекта
    }
}