// Контроллер управляет выборами, закрытием голосования и получением результатов.

using Inter.MixAN.Data;
using Inter.MixAN.Domain;
using Inter.MixAN.DTOs;
using Inter.MixAN.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inter.MixAN.Controllers;

[ApiController]
[Route("api/elections")]
public class ElectionsController : ControllerBase
{
    private readonly VotingDbContext _dbContext;
    private readonly VotingService _votingService;
    private readonly AuditService _audit;

    public ElectionsController(VotingDbContext dbContext, VotingService votingService, AuditService audit)
    {
        _dbContext = dbContext;
        _votingService = votingService;
        _audit = audit;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Election>>> GetAll() => Ok(await _dbContext.Elections.OrderBy(x => x.Id).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Election>> GetById(int id)
    {
        var election = await _dbContext.Elections.FindAsync(id);
        return election is null ? NotFound() : Ok(election);
    }

    [HttpPost]
    public async Task<ActionResult<Election>> Create([FromBody] CreateElectionDto dto)
    {
        if (dto.EndUtc <= dto.StartUtc)
            return BadRequest("EndUtc must be greater than StartUtc.");

        var election = new Election
        {
            Title = dto.Title,
            Description = dto.Description,
            StartUtc = dto.StartUtc,
            EndUtc = dto.EndUtc,
            IsClosed = false
        };

        _dbContext.Elections.Add(election);
        await _dbContext.SaveChangesAsync();
        await _audit.WriteAsync("ElectionCreated", nameof(Election), election.Id, $"Election {election.Title} created.");
        return CreatedAtAction(nameof(GetById), new { id = election.Id }, election);
    }

    [HttpPost("{id:int}/close")]
    public async Task<ActionResult<Election>> Close(int id)
    {
        try
        {
            return Ok(await _votingService.CloseElectionAsync(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id:int}/results")]
    public async Task<ActionResult<ElectionResult>> GetResults(int id)
    {
        try
        {
            return Ok(await _votingService.GetResultAsync(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
