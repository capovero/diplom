using System.ComponentModel.DataAnnotations;

namespace backendtest.Models;

public class Review
{
    public int Id { get; set; } // ID отзыва
    public int ProjectId { get; set; } // Ссылка на проект
    
    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
    public int Rating { get; set; } // Оценка от 0 до 5
    public string Comment { get; set; } = string.Empty; // Текст отзыва

    public Project Project { get; set; } = null!; // Навигационное свойство
    
    public Guid UserId { get; set; } // Связь с пользователем
    public User User { get; set; } = null!;
}