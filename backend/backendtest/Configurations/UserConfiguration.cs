using backendtest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace backendtest.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Указываем первичный ключ
        builder.HasKey(u => u.Id);

        // Указываем максимальную длину имени пользователя
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(User.MAX_USERNAME_LENGTH);

        // Указываем, что Email обязателен
        builder.Property(u => u.Email).IsRequired();

        // Навигационное свойство: один пользователь может создать много проектов
        builder.HasMany<Project>(u => u.Projects)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Удаление пользователя удаляет его проекты

        // Навигационное свойство: один пользователь может сделать много пожертвований
        builder.HasMany<Donation>(u => u.Donations)
            .WithOne(d => d.User)
            .HasForeignKey(d => d.UserId);
    }
}