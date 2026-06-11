// Контроллер возвращает журнал аудита с действиями пользователей и системы.

using Inter.MixAN.Data;
using Inter.MixAN.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inter.MixAN.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly VotingDbContext _dbContext;

    public AuditController(VotingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuditLog>>> GetAll() =>
        Ok(await _dbContext.AuditLogs.OrderByDescending(x => x.CreatedUtc).ToListAsync());
}
