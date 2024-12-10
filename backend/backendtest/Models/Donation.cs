namespace backendtest.Models;

public class Donation //     Описание: Хранит информацию о пожертвованиях. 
{
    public int Id { get; set; } // - `Id` (int) — первичный ключ.
    public decimal Amount { get; set; } // - `Amount` (decimal) — сумма пожертвования.
    public DateTime DonateAt { get; set; } // - `DonatedAt` (DateTime) — дата пожертвования.
    
    public Project Project { get; set; }
    public int ProjectId { get; set; } // - `ProjectId` (int) — проект, которому сделано пожертвование (внешний ключ).
    
    public User User { get; set; }
    public Guid UserId { get; set; } // - `UserId` (int) — пользователь, который сделал пожертвование (внешний ключ).
}

// - Одно пожертвование связано с одним проектом и одним пользователем.