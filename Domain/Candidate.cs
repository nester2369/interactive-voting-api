// Модель описывает кандидата и его связи с голосами и заявками.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.Domain;

public class Candidate
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    [MaxLength(4000)]
    public string Biography { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Program { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Slogan { get; set; } = string.Empty;

    public bool IsApproved { get; set; }

    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    public ICollection<CandidateApplication> Applications { get; set; } = new List<CandidateApplication>();
}
