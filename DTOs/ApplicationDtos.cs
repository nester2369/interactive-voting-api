// DTO-модели задают входные данные для подачи и рассмотрения заявки кандидата.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.DTOs;

public class CreateApplicationDto
{
    [Required]
    public int CandidateId { get; set; }
}

public class ReviewApplicationDto
{
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;
}
