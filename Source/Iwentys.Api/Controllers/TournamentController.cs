using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities;
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
        public IEnumerable<Tournament> Get()
        {
            return _tournamentService.Get();
        }

        [HttpGet("{id}")]
        public Tournament Get(int id)
        {
            return _tournamentService.Get(id);
        }
    }
}
