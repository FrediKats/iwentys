using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Guilds.Tournaments.Services;
using Iwentys.Features.Students.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly TournamentService _tournamentService;

        public TournamentController(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TournamentInfoResponse>>> Get()
        {
            List<TournamentInfoResponse> tournaments = await _tournamentService.GetAsync();
            return Ok(tournaments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentInfoResponse>> Get(int id)
        {
            TournamentInfoResponse tournament = await _tournamentService.GetAsync(id);
            return Ok(tournament);
        }

        [HttpPost("code-marathon")]
        public async Task<ActionResult<TournamentInfoResponse>> CreateCodeMarathon([FromBody] CreateCodeMarathonTournamentArguments arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TournamentInfoResponse tournamentInfoResponse = await _tournamentService.CreateCodeMarathon(user, arguments);
            return Ok(tournamentInfoResponse);
        }
    }
}