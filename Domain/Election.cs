// Модель описывает выборы, период голосования и состояние закрытия.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.Domain;

public class Election
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public bool IsClosed { get; set; }

    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
