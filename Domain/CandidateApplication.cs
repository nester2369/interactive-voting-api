// Модель описывает заявку кандидата и статус ее рассмотрения.

using System.ComponentModel.DataAnnotations;

namespace Inter.MixAN.Domain;

public enum ApplicationStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

public class CandidateApplication
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public DateTime SubmittedUtc { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    public DateTime? ReviewedUtc { get; set; }
    public Candidate? Candidate { get; set; }
}
