using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TournamentEntity>> Get()
        {
            return Ok(_tournamentService.Get());
        }

        [HttpGet("{id}")]
        public ActionResult<TournamentEntity> Get(int id)
        {
            return Ok(_tournamentService.Get(id));
        }
    }
}