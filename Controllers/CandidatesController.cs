// Контроллер управляет кандидатами: создание, просмотр, изменение и удаление записей.

using Inter.MixAN.Data;
using Inter.MixAN.Domain;
using Inter.MixAN.DTOs;
using Inter.MixAN.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inter.MixAN.Controllers;

[ApiController]
[Route("api/candidates")]
public class CandidatesController : ControllerBase
{
    private readonly VotingDbContext _dbContext;
    private readonly AuditService _audit;

    public CandidatesController(VotingDbContext dbContext, AuditService audit)
    {
        _dbContext = dbContext;
        _audit = audit;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Candidate>>> GetAll() => Ok(await _dbContext.Candidates.OrderBy(x => x.Id).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Candidate>> GetById(int id)
    {
        var candidate = await _dbContext.Candidates.FindAsync(id);
        return candidate is null ? NotFound() : Ok(candidate);
    }

    [HttpPost]
    public async Task<ActionResult<Candidate>> Create([FromBody] CreateCandidateDto dto)
    {
        var candidate = new Candidate
        {
            FullName = dto.FullName,
            DateOfBirth = dto.DateOfBirth,
            Biography = dto.Biography,
            Program = dto.Program,
            Slogan = dto.Slogan,
            IsApproved = false
        };

        _dbContext.Candidates.Add(candidate);
        await _dbContext.SaveChangesAsync();
        await _audit.WriteAsync("CandidateCreated", nameof(Candidate), candidate.Id, $"Candidate {candidate.FullName} created.");
        return CreatedAtAction(nameof(GetById), new { id = candidate.Id }, candidate);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Candidate>> Update(int id, [FromBody] CreateCandidateDto dto)
    {
        var candidate = await _dbContext.Candidates.FindAsync(id);
        if (candidate is null)
            return NotFound();

        candidate.FullName = dto.FullName;
        candidate.DateOfBirth = dto.DateOfBirth;
        candidate.Biography = dto.Biography;
        candidate.Program = dto.Program;
        candidate.Slogan = dto.Slogan;

        await _dbContext.SaveChangesAsync();
        await _audit.WriteAsync("CandidateUpdated", nameof(Candidate), candidate.Id, $"Candidate {candidate.FullName} updated.");
        return Ok(candidate);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var candidate = await _dbContext.Candidates.FindAsync(id);
        if (candidate is null)
            return NotFound();

        _dbContext.Candidates.Remove(candidate);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
