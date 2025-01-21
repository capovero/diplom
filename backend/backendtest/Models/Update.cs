using System.ComponentModel.DataAnnotations.Schema;

namespace backendtest.Models;

public class Update
{
    public int Id { get; set; } 
    public string Content { get; set; } = String.Empty; 
    public DateTime CreatedAt { get; set; }
    public Project Project { get; set; }
    public int ProjectId { get; set; } 
}
