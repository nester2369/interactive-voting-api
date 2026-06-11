// DTO-модель задает входные данные для создания или изменения кандидата.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.DTOs;

public class CreateCandidateDto
{
    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [MaxLength(4000)]
    public string Biography { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Program { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Slogan { get; set; } = string.Empty;
}
