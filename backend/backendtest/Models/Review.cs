namespace backendtest.Models;

public class Review
{
    public int Id { get; set; } // ID отзыва
    public int ProjectId { get; set; } // Ссылка на проект
    public int Rating { get; set; } // Оценка от 0 до 5
    public string Comment { get; set; } = string.Empty; // Текст отзыва

    public Project Project { get; set; } = null!; // Навигационное свойство
}