using System.ComponentModel.DataAnnotations.Schema;

namespace backendtest.Models;

public class Project 
{
    public int Id { get; set; } // Id (int) — первичный ключ.
    public string Title { get; set; } = string.Empty; // Название проекта.
    [Column(TypeName = "text")]
    public string Description { get; set; } = string.Empty; // Описание проекта.
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal GoalAmount { get; set; } // Цель сбора средств.

    [Column(TypeName = "decimal(18,2)")]
    public decimal CollectedAmount { get; set; } = 0; // Сумма собранных средств.
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;// Дата создания.
    public DateTime UpdatedAt { get; set; } // Дата обновления.
    
    // Хранение статуса как перечисления
    public Status Status { get; set; } = Status.Pending; // Использование enum
    
    // Внешний ключ и связь с пользователем
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    // Внешний ключ и связь с категорией
    public int? CategoryId { get; set; }
    public Category Category { get; set; }
    
    // Проект может иметь много обновлений
    public ICollection<Update> Updates { get; set; } = new List<Update>();

    // Проект может иметь много вложений (медиа)
    public ICollection<Media> MediaFiles { get; set; } = new List<Media>();

    // Проект может иметь много пожертвований
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    
    public float AverageRating { get; set; } = 0; // Новая колонка

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    
}

