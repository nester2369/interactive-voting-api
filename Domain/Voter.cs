// Модель описывает избирателя и его статус проверки.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.Domain;

public class Voter
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    [Required, MaxLength(50)]
    public string PassportNumber { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsVerified { get; set; }

    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
