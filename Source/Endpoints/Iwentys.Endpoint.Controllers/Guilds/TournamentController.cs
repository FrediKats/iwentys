using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Guilds.Tournaments.Services;
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
    }
}