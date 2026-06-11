// Модели описывают итоговые результаты выборов и распределение голосов по кандидатам.

namespace Inter.MixAN.Domain;

public class ElectionResult
{
    public int ElectionId { get; set; }
    public string ElectionTitle { get; set; } = string.Empty;
    public int TotalVotes { get; set; }
    public List<CandidateResult> Candidates { get; set; } = new();
}

public class CandidateResult
{
    public int CandidateId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public int Votes { get; set; }
    public double Percent { get; set; }
}
