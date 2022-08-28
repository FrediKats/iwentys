using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.PeerReview;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Guilds;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, GithubIntegrationService githubIntegrationService, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _githubIntegrationService = githubIntegrationService;
            _entityManagerApiClient = entityManagerApiClient;
        }

        protected override Response Handle(Query request)
        {
            IwentysUser student = _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id).Result;
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