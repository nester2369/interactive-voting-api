// Модель описывает голос избирателя за кандидата в рамках конкретных выборов.

namespace Inter.MixAN.Domain;

public class Vote
{
    public int Id { get; set; }
    public int ElectionId { get; set; }
    public int VoterId { get; set; }
    public int CandidateId { get; set; }
    public DateTime CreatedUtc { get; set; }

    public Election? Election { get; set; }
    public Voter? Voter { get; set; }
    public Candidate? Candidate { get; set; }
}
