using System.ComponentModel.DataAnnotations.Schema;
using backendtest.Models;

namespace backendtest.Dtos.UpdateDto;

public class UpdateDto
{
    [Column(TypeName = "text")]
    public string Content { get; set; } = String.Empty; 
    public int ProjectId { get; set; } 
}