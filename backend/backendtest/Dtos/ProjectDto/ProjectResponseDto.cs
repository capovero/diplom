using backendtest.Models;

namespace backendtest.Dtos.ProjectDto;

public class ProjectResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal GoalAmount { get; set; }
    public decimal CollectedAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Status status { get; set; }
    
    public int? CategoryId { get; set; }
    public float AverageRating { get; set; } 
    public List<string> MediaFiles { get; set; }
}