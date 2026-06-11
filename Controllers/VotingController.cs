// Контроллер принимает голос избирателя и передает проверку правил в сервис голосования.

using Inter.MixAN.Domain;
using Inter.MixAN.DTOs;
using Inter.MixAN.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inter.MixAN.Controllers;

[ApiController]
[Route("api/voting")]
public class VotingController : ControllerBase
{
    private readonly VotingService _votingService;

    public VotingController(VotingService votingService)
    {
        _votingService = votingService;
    }

    [HttpPost("cast")]
    public async Task<ActionResult<Vote>> CastVote([FromBody] CastVoteDto dto)
    {
        try
        {
            return Ok(await _votingService.CastVoteAsync(dto.ElectionId, dto.VoterId, dto.CandidateId));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
