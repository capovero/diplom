using backendtest.Models;

namespace backendtest.Dtos.UpdateDto;

public class UpdateResponseDto
{
    public int Id { get; set; } 
    public string Content { get; set; } = String.Empty; 
    public DateTime CreatedAt { get; set; }
}