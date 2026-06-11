// Контроллер обрабатывает заявки кандидатов и операции их одобрения или отклонения.

using Inter.MixAN.Data;
using Inter.MixAN.Domain;
using Inter.MixAN.DTOs;
using Inter.MixAN.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inter.MixAN.Controllers;

[ApiController]
[Route("api/applications")]
public class ApplicationsController : ControllerBase
{
    private readonly VotingDbContext _dbContext;
    private readonly ApplicationService _service;

    public ApplicationsController(VotingDbContext dbContext, ApplicationService service)
    {
        _dbContext = dbContext;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CandidateApplication>>> GetAll() =>
        Ok(await _dbContext.CandidateApplications.OrderByDescending(x => x.SubmittedUtc).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CandidateApplication>> GetById(int id)
    {
        var application = await _dbContext.CandidateApplications.FindAsync(id);
        return application is null ? NotFound() : Ok(application);
    }

    [HttpPost]
    public async Task<ActionResult<CandidateApplication>> Create([FromBody] CreateApplicationDto dto)
    {
        try
        {
            var created = await _service.SubmitAsync(dto.CandidateId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidOperationException)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:int}/approve")]
    public async Task<ActionResult<CandidateApplication>> Approve(int id, [FromBody] ReviewApplicationDto dto)
    {
        try
        {
            return Ok(await _service.ApproveAsync(id, dto.Comment));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{id:int}/reject")]
    public async Task<ActionResult<CandidateApplication>> Reject(int id, [FromBody] ReviewApplicationDto dto)
    {
        try
        {
            return Ok(await _service.RejectAsync(id, dto.Comment));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
