namespace backendtest.Models;

public class Media // Описание: Хранит фотографии и видео, связанные с проектами.  
{
    public int Id { get; set; }// - `Id` (int) — первичный ключ.
    public string FilePath { get; set; } = String.Empty; // - `FilePath` (string) — путь к файлу.
    
    public Project Project { get; set; }
    public int ProjectId { get; set; } // - `ProjectId` (int) — проект, к которому привязано вложение (внешний ключ).
}
// - Одно вложение связано с одним проектом.