using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.Guilds.Dtos;
using Iwentys.Modules.Guilds.Tournaments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Guilds.Tournaments
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
        public async Task<ActionResult<List<TournamentInfoResponse>>> Get(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetTournaments.Response response = await _mediator.Send(new GetTournaments.Query(user));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<TournamentInfoResponse>
                .ToIndexViewModel(response.Tournaments, paginationFilter));
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