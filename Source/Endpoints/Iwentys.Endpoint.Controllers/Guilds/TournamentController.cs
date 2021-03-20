using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
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

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<TournamentInfoResponse>>> Get()
        {
            List<TournamentInfoResponse> tournaments = await _tournamentService.Get();
            return Ok(tournaments);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<TournamentInfoResponse>> GetById(int id)
        {
            TournamentInfoResponse tournament = await _tournamentService.Get(id);
            return Ok(tournament);
        }

        [HttpGet(nameof(FindActiveByGuildId))]
        public async Task<ActionResult<TournamentInfoResponse>> FindActiveByGuildId(int guildId)
        {
            TournamentInfoResponse tournament = await _tournamentService.FindGuildActiveTournament(guildId);
            if (tournament is null)
                return NotFound();

            return Ok(tournament);
        }

        [HttpPost(nameof(CreateCodeMarathon))]
        public async Task<ActionResult<TournamentInfoResponse>> CreateCodeMarathon([FromBody] CreateCodeMarathonTournamentArguments arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TournamentInfoResponse tournamentInfoResponse = await _tournamentService.CreateCodeMarathon(user, arguments);
            return Ok(tournamentInfoResponse);
        }

        [HttpPut(nameof(RegisterToTournament))]
        public async Task<ActionResult> RegisterToTournament(int tournamentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _tournamentService.RegisterToTournament(user, tournamentId);
            return Ok();
        }

        //TODO: move to debug controller?
        [HttpGet(nameof(ForceUpdate))]
        public async Task ForceUpdate(int tournamentId)
        {
            await _tournamentService.UpdateResult(tournamentId);
        }
    }
}