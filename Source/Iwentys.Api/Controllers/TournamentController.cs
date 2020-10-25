using System.Collections.Generic;
using Iwentys.Core.Services;
using Iwentys.Models.Transferable;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly TournamentService _tournamentService;

        public TournamentController(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TournamentInfoResponse>> Get()
        {
            return Ok(_tournamentService.Get());
        }

        [HttpGet("{id}")]
        public ActionResult<TournamentInfoResponse> Get(int id)
        {
            return Ok(_tournamentService.Get(id));
        }
    }
}