using System.ComponentModel.DataAnnotations;

namespace backendtest.Dtos.ReviewDto;

public class UpdateReviewDto
{
    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
    public int Rating { get; set; } // Оценка от 0 до 5
    public string Comment { get; set; } = string.Empty; // Текст отзыва
} 