using System.ComponentModel.DataAnnotations;

namespace backendtest.Dtos.ReviewDto;

public class ReviewResponseDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    
    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
    public int Rating { get; set; }
    [Required(ErrorMessage = "Comment is required.")]
    [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
    public string Comment { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty; 
    public string ProjectName { get; set; } = string.Empty; 
}
