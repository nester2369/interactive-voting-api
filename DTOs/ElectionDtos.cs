// DTO-модель задает входные данные для создания выборов.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.DTOs;

public class CreateElectionDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartUtc { get; set; }

    [Required]
    public DateTime EndUtc { get; set; }
}
