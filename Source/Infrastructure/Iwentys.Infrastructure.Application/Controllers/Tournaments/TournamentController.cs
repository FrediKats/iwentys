using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Guilds.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.Tournaments
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TournamentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<TournamentInfoResponse>>> Get()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetTournaments.Response response = await _mediator.Send(new GetTournaments.Query(user));
            return Ok(response.Tournaments);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<TournamentInfoResponse>> GetById(int id)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetTournamentById.Response response = await _mediator.Send(new GetTournamentById.Query(user, id));
            return Ok(response.Tournament);
        }

        [HttpGet(nameof(FindActiveByGuildId))]
        public async Task<ActionResult<TournamentInfoResponse>> FindActiveByGuildId(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            FindGuildActiveTournament.Response response = await _mediator.Send(new FindGuildActiveTournament.Query(user, guildId));
            return Ok(response.Tournament);
        }

        [HttpPost(nameof(CreateCodeMarathon))]
        public async Task<ActionResult> CreateCodeMarathon([FromBody] CreateCodeMarathonTournamentArguments arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            CreateCodeMarathon.Response response = await _mediator.Send(new CreateCodeMarathon.Query(user, arguments));
            return Ok();
        }

        [HttpPut(nameof(RegisterToTournament))]
        public async Task<ActionResult> RegisterToTournament(int tournamentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            RegisterToTournament.Response response = await _mediator.Send(new RegisterToTournament.Query(user, tournamentId));
            return Ok();
        }

        //TODO: move to debug controller?
        [HttpGet(nameof(ForceUpdate))]
        public async Task ForceUpdate(int tournamentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            ForceTournamentResultUpdate.Response response = await _mediator.Send(new ForceTournamentResultUpdate.Query(user, tournamentId));
        }
    }
}