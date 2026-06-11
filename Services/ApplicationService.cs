// Сервис содержит бизнес-логику подачи, одобрения и отклонения заявок кандидатов.

using Inter.MixAN.Data;
using Inter.MixAN.Domain;
using Microsoft.EntityFrameworkCore;

namespace Inter.MixAN.Services;

public class ApplicationService
{
    private readonly VotingDbContext _dbContext;
    private readonly AuditService _auditService;

    public ApplicationService(VotingDbContext dbContext, AuditService auditService)
    {
        _dbContext = dbContext;
        _auditService = auditService;
    }

    public async Task<CandidateApplication> SubmitAsync(int candidateId)
    {
        var candidate = await _dbContext.Candidates.FirstOrDefaultAsync(x => x.Id == candidateId)
            ?? throw new KeyNotFoundException("Candidate not found.");

        var existingPending = await _dbContext.CandidateApplications
            .AnyAsync(x => x.CandidateId == candidateId && x.Status == ApplicationStatus.Pending);

        if (existingPending)
        {
            throw new InvalidOperationException("Candidate already has a pending application.");
        }

        var application = new CandidateApplication
        {
            CandidateId = candidateId,
            SubmittedUtc = DateTime.UtcNow,
            Status = ApplicationStatus.Pending
        };

        _dbContext.CandidateApplications.Add(application);
        await _dbContext.SaveChangesAsync();
        await _auditService.WriteAsync("ApplicationSubmitted", nameof(CandidateApplication), application.Id, $"Candidate {candidate.FullName} submitted an application.");
        return application;
    }

    public async Task<CandidateApplication> ApproveAsync(int applicationId, string comment)
    {
        var application = await _dbContext.CandidateApplications.FirstOrDefaultAsync(x => x.Id == applicationId)
            ?? throw new KeyNotFoundException("Application not found.");
        var candidate = await _dbContext.Candidates.FirstOrDefaultAsync(x => x.Id == application.CandidateId)
            ?? throw new KeyNotFoundException("Candidate not found.");

        application.Status = ApplicationStatus.Approved;
        application.Comment = comment;
        application.ReviewedUtc = DateTime.UtcNow;
        candidate.IsApproved = true;

        await _dbContext.SaveChangesAsync();
        await _auditService.WriteAsync("ApplicationApproved", nameof(CandidateApplication), application.Id, $"Candidate {candidate.FullName} approved.");
        return application;
    }

    public async Task<CandidateApplication> RejectAsync(int applicationId, string comment)
    {
        var application = await _dbContext.CandidateApplications.FirstOrDefaultAsync(x => x.Id == applicationId)
            ?? throw new KeyNotFoundException("Application not found.");
        var candidate = await _dbContext.Candidates.FirstOrDefaultAsync(x => x.Id == application.CandidateId)
            ?? throw new KeyNotFoundException("Candidate not found.");

        application.Status = ApplicationStatus.Rejected;
        application.Comment = comment;
        application.ReviewedUtc = DateTime.UtcNow;
        candidate.IsApproved = false;

        await _dbContext.SaveChangesAsync();
        await _auditService.WriteAsync("ApplicationRejected", nameof(CandidateApplication), application.Id, $"Candidate {candidate.FullName} rejected.");
        return application;
    }
}
