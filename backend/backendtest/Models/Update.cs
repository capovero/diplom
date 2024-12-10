using System.ComponentModel.DataAnnotations.Schema;

namespace backendtest.Models;

public class Update //Описание: Хранит обновления по проекту.  
{
    public int Id { get; set; } // - `Id` (int) — первичный ключ.
    [Column(TypeName = "text")]
    public string Content { get; set; } = String.Empty; // - `Content` (text) — содержание обновления.
    public DateTime CreatedAt { get; set; } // - `CreatedAt` (DateTime) — дата создания.
    
    public Project Project { get; set; }
    public int ProjectId { get; set; } // - `ProjectId` (int) — проект, к которому относится обновление (внешний ключ).
}
// Может быть много обновлений на каждый проект