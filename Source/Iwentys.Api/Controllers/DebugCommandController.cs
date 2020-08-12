
﻿using System;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Iwentys.Core.Auth;
using Iwentys.Core.GoogleTableParsing;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugCommandController : ControllerBase
    {
        private readonly ISubjectActivityRepository _subjectActivityRepository;
        private readonly ISubjectForGroupRepository _subjectForGroupRepository;
        private readonly GoogleTableUpdateService _googleTableUpdateService;

        private readonly IStudentRepository _studentRepository;
        private IConfiguration _configuration;


        public DebugCommandController(ISubjectActivityRepository subjectActivityRepository, ISubjectForGroupRepository subjectForGroupRepository, IConfiguration configuration, IStudentRepository studentRepository)
        {
            _subjectActivityRepository = subjectActivityRepository;
            _subjectForGroupRepository = subjectForGroupRepository;

            _googleTableUpdateService = new GoogleTableUpdateService(_subjectActivityRepository, configuration);
            _configuration = configuration;
            _studentRepository = studentRepository;

        }

        [HttpPost("UpdateSubjectActivityData")]
        public void UpdateSubjectActivityData(SubjectActivity activity)
        {
            _subjectActivityRepository.Update(activity);
        }

        [HttpPost("UpdateSubjectActivityForGroup")]
        public void UpdateSubjectActivityForGroup(int subjectId, int groupId)
        {
            SubjectForGroup subjectData = _subjectForGroupRepository
                .Read()
                .FirstOrDefault(s => s.SubjectId == subjectId && s.StudyGroupId == groupId);

            if (subjectData == null)
            {
                // TODO: Some logs
                return;
            }
            
            _googleTableUpdateService.UpdateSubjectActivityForGroup(subjectData);
        }

        [HttpGet("login/{userId}")]
        public string Login(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            _studentRepository.Get(userId);
            return GenerateToken(userId, signingEncodingKey);
        }

        [HttpPost("register")]
        public string Register([FromBody] StudentCreateArgumentsDto arguments,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            var student = new Student()
            {
                Id = arguments.Id,
                FirstName = arguments.FirstName,
                MiddleName = arguments.MiddleName,
                SecondName = arguments.SecondName,
                Role = arguments.Role,
                Group = arguments.Group,
                GithubUsername = arguments.GithubUsername,
                CreationTime = DateTime.UtcNow,
                LastOnlineTime = DateTime.UtcNow,
                BarsPoints = arguments.BarsPoints,
                GuildLeftTime = DateTime.MinValue.ToUniversalTime()
            };

            _studentRepository.Create(student);

            return GenerateToken(student.Id, signingEncodingKey);
        }

        private string GenerateToken(int userId, IJwtSigningEncodingKey signingEncodingKey)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.UserData, userId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "Iwentys",
                audience: "IwentysWeb",
                claims: claims,
                signingCredentials: new SigningCredentials(
                    signingEncodingKey.GetKey(),
                    signingEncodingKey.SigningAlgorithm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
