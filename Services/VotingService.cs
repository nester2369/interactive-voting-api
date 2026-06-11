// Сервис содержит правила голосования, подсчет результатов и закрытие выборов.

using Inter.MixAN.Data;
using Inter.MixAN.Domain;
using Microsoft.EntityFrameworkCore;

namespace Inter.MixAN.Services;

public class VotingService
{
    private readonly VotingDbContext _dbContext;
    private readonly AuditService _auditService;

    public VotingService(VotingDbContext dbContext, AuditService auditService)
    {
        _dbContext = dbContext;
        _auditService = auditService;
    }

    public async Task<Vote> CastVoteAsync(int electionId, int voterId, int candidateId)
    {
        var election = await _dbContext.Elections.FirstOrDefaultAsync(x => x.Id == electionId)
            ?? throw new KeyNotFoundException("Election not found.");
        var voter = await _dbContext.Voters.FirstOrDefaultAsync(x => x.Id == voterId)
            ?? throw new KeyNotFoundException("Voter not found.");
        var candidate = await _dbContext.Candidates.FirstOrDefaultAsync(x => x.Id == candidateId)
            ?? throw new KeyNotFoundException("Candidate not found.");

        if (!voter.IsVerified)
            throw new InvalidOperationException("Voter is not verified.");

        if (!candidate.IsApproved)
            throw new InvalidOperationException("Candidate is not approved.");

        var now = DateTime.UtcNow;
        if (election.IsClosed || now < election.StartUtc || now > election.EndUtc)
            throw new InvalidOperationException("Election is not active.");

        var duplicate = await _dbContext.Votes.AnyAsync(x => x.ElectionId == electionId && x.VoterId == voterId);
        if (duplicate)
            throw new InvalidOperationException("This voter has already voted in the election.");

        var vote = new Vote
        {
            ElectionId = electionId,
            VoterId = voterId,
            CandidateId = candidateId,
            CreatedUtc = now
        };

        _dbContext.Votes.Add(vote);
        await _dbContext.SaveChangesAsync();
        await _auditService.WriteAsync("VoteCast", nameof(Vote), vote.Id, $"Voter {voterId} voted for candidate {candidateId} in election {electionId}.");
        return vote;
    }

    public async Task<ElectionResult> GetResultAsync(int electionId)
    {
        var election = await _dbContext.Elections.FirstOrDefaultAsync(x => x.Id == electionId)
            ?? throw new KeyNotFoundException("Election not found.");

        var electionVotes = await _dbContext.Votes
            .Where(x => x.ElectionId == electionId)
            .ToListAsync();

        var totalVotes = electionVotes.Count;
        var candidates = await _dbContext.Candidates
            .Where(x => x.IsApproved)
            .OrderBy(x => x.FullName)
            .ToListAsync();

        return new ElectionResult
        {
            ElectionId = election.Id,
            ElectionTitle = election.Title,
            TotalVotes = totalVotes,
            Candidates = candidates
                .Select(candidate =>
                {
                    var count = electionVotes.Count(x => x.CandidateId == candidate.Id);
                    return new CandidateResult
                    {
                        CandidateId = candidate.Id,
                        CandidateName = candidate.FullName,
                        Votes = count,
                        Percent = totalVotes == 0 ? 0 : Math.Round(count * 100.0 / totalVotes, 2)
                    };
                })
                .OrderByDescending(x => x.Votes)
                .ThenBy(x => x.CandidateName)
                .ToList()
        };
    }

    public async Task<Election> CloseElectionAsync(int electionId)
    {
        var election = await _dbContext.Elections.FirstOrDefaultAsync(x => x.Id == electionId)
            ?? throw new KeyNotFoundException("Election not found.");

        election.IsClosed = true;
        await _dbContext.SaveChangesAsync();
        await _auditService.WriteAsync("ElectionClosed", nameof(Election), election.Id, $"Election {election.Title} closed.");
        return election;
    }
}
