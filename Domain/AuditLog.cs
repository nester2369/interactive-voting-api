// Модель описывает запись журнала аудита.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.Domain;

public class AuditLog
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string EntityName { get; set; } = string.Empty;

    public int EntityId { get; set; }

    [MaxLength(2000)]
    public string Details { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; }
}
