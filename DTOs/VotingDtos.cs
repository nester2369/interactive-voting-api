// DTO-модель задает входные данные для отправки голоса.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.DTOs;

public class CastVoteDto
{
    [Required]
    public int ElectionId { get; set; }

    [Required]
    public int VoterId { get; set; }

    [Required]
    public int CandidateId { get; set; }
}
