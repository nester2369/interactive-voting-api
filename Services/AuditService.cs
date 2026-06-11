// Сервис записывает действия системы в журнал аудита.

using Inter.MixAN.Data;
using Inter.MixAN.Domain;

namespace Inter.MixAN.Services;

public class AuditService
{
    private readonly VotingDbContext _dbContext;

    public AuditService(VotingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AuditLog> WriteAsync(string action, string entityName, int entityId, string details)
    {
        var log = new AuditLog
        {
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Details = details,
            CreatedUtc = DateTime.UtcNow
        };

        _dbContext.AuditLogs.Add(log);
        await _dbContext.SaveChangesAsync();
        return log;
    }
}
