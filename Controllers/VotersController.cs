// Контроллер управляет избирателями и подтверждением их допуска к голосованию.

using Inter.MixAN.Data;
using Inter.MixAN.Domain;
using Inter.MixAN.DTOs;
using Inter.MixAN.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inter.MixAN.Controllers;

[ApiController]
[Route("api/voters")]
public class VotersController : ControllerBase
{
    private readonly VotingDbContext _dbContext;
    private readonly AuditService _audit;

    public VotersController(VotingDbContext dbContext, AuditService audit)
    {
        _dbContext = dbContext;
        _audit = audit;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Voter>>> GetAll() => Ok(await _dbContext.Voters.OrderBy(x => x.Id).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Voter>> GetById(int id)
    {
        var voter = await _dbContext.Voters.FindAsync(id);
        return voter is null ? NotFound() : Ok(voter);
    }

    [HttpPost]
    public async Task<ActionResult<Voter>> Create([FromBody] CreateVoterDto dto)
    {
        var exists = await _dbContext.Voters.AnyAsync(x => x.PassportNumber == dto.PassportNumber || x.Email == dto.Email);
        if (exists)
            return Conflict("Voter with the same passport number or email already exists.");

        var voter = new Voter
        {
            FullName = dto.FullName,
            DateOfBirth = dto.DateOfBirth,
            PassportNumber = dto.PassportNumber,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            IsVerified = false
        };

        _dbContext.Voters.Add(voter);
        await _dbContext.SaveChangesAsync();
        await _audit.WriteAsync("VoterCreated", nameof(Voter), voter.Id, $"Voter {voter.FullName} created.");
        return CreatedAtAction(nameof(GetById), new { id = voter.Id }, voter);
    }

    [HttpPost("{id:int}/verify")]
    public async Task<ActionResult<Voter>> Verify(int id)
    {
        var voter = await _dbContext.Voters.FindAsync(id);
        if (voter is null)
            return NotFound();

        voter.IsVerified = true;
        await _dbContext.SaveChangesAsync();
        await _audit.WriteAsync("VoterVerified", nameof(Voter), voter.Id, $"Voter {voter.FullName} verified.");
        return Ok(voter);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var voter = await _dbContext.Voters.FindAsync(id);
        if (voter is null)
            return NotFound();

        _dbContext.Voters.Remove(voter);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
