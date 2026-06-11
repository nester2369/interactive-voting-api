// DTO-модель задает входные данные для создания избирателя.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.DTOs;

public class CreateVoterDto
{
    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required, MaxLength(50)]
    public string PassportNumber { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;
}
