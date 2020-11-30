﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Guilds.ViewModels.Tournaments;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers
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
        public async Task<ActionResult<IEnumerable<TournamentInfoResponse>>> Get()
        {
            List<TournamentInfoResponse> tournaments = await _tournamentService.Get();
            return Ok(tournaments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentInfoResponse>> Get(int id)
        {
            TournamentInfoResponse tournament = await _tournamentService.Get(id);
            return Ok(tournament);
        }
    }
}