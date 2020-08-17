﻿using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Gamification;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IQuestService _questService;

        public QuestController(IQuestService questService)
        {
            _questService = questService;
        }

        [HttpGet("GetCreatedByUser")]
        public ActionResult<List<QuestInfoDto>> GetCreatedByUser()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_questService.GetCreatedByUser(user));
        }

        [HttpGet("GetCompletedByUser")]
        public ActionResult<List<QuestInfoDto>> GetCompletedByUser()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_questService.GetCompletedByUser(user));
        }

        [HttpGet("GetActive")]
        public ActionResult<List<QuestInfoDto>> GetActive()
        {
            return Ok(_questService.GetActive());
        }

        [HttpGet("GetArchived")]
        public ActionResult<List<QuestInfoDto>> GetArchived()
        {
            return Ok(_questService.GetArchived());
        }

        [HttpPost("Create")]
        public ActionResult<QuestInfoDto> Create(CreateQuestDto createQuest)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_questService.Create(user, createQuest));
        }

        [HttpPost("{questId}/SendResponse")]
        public ActionResult<QuestInfoDto> SendResponse(int questId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_questService.SendResponse(user, questId));
        }

        [HttpPost("{questId}/SetCompleted")]
        public ActionResult<QuestInfoDto> SetCompleted([FromRoute]int questId, [FromQuery] int userId)
        {
            AuthorizedUser author = AuthorizedUser.DebugAuth();
            return Ok(_questService.SetCompleted(author, questId, userId));
        }
    }
}