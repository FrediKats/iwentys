using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.PeerReview.Dto;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Authorization;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Guilds.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.Guilds.GuildTributes.Queries
{
    public class CreateTribute
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, CreateProjectRequestDto arguments)
            {
                User = user;
                Arguments = arguments;
            }

            public AuthorizedUser User { get; set; }
            public CreateProjectRequestDto Arguments { get; set; }
        }

        public class Response
        {
            public Response(TributeInfoResponse tribute)
            {
                Tribute = tribute;
            }

            public TributeInfoResponse Tribute { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;
            private readonly GithubIntegrationService _githubIntegrationService;

            public Handler(IwentysDbContext context, GithubIntegrationService githubIntegrationService)
            {
                _context = context;
                _githubIntegrationService = githubIntegrationService;
            }

            protected override Response Handle(Query request)
            {
                IwentysUser student = _context.IwentysUsers.GetById(request.User.Id).Result;
                GithubRepositoryInfoDto githubProject = _githubIntegrationService.Repository.GetRepository(request.Arguments.Owner, request.Arguments.RepositoryName).Result;
                GithubProject project = GetOrCreate(githubProject, student).Result;
                Guild guild = _context.GuildMembers.ReadForStudent(student.Id).Result;
                List<Tribute> allTributes = _context.Tributes.ToListAsync().Result;

                var tribute = Tribute.Create(guild, student, project);

                _context.Tributes.Add(tribute);

                TributeInfoResponse tributeInfoResponse = _context.Tributes
                    .Where(t => t.ProjectId == tribute.ProjectId)
                    .Select(TributeInfoResponse.FromEntity)
                    .SingleAsync().Result;

                return new Response(tributeInfoResponse);
            }

            public async Task<GithubProject> GetOrCreate(GithubRepositoryInfoDto project, IwentysUser creator)
            {
                GithubProject githubProject = _context.StudentProjects.Find(project.Id);
                if (githubProject is not null)
                    return githubProject;

                GithubUser githubUser = await _githubIntegrationService.User.Get(creator.Id);
                //TODO: need to get this from GithubService
                var newProject = new GithubProject(githubUser, project);

                _context.StudentProjects.Add(newProject);
                return newProject;
            }
        }
    }
}