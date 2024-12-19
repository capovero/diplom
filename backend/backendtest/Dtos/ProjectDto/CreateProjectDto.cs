using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backendtest.Models;

namespace backendtest.Dtos.ProjectDto;

public class CreateProjectDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "GoalAmount must be greater than 0.")]
    public decimal GoalAmount { get; set; }
    
    public int? CategoryId { get; set; }
    
    [Required]
    public List<IFormFile> MediaFiles { get; set; } = new();
}