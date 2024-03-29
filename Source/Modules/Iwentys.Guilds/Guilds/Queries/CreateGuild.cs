﻿using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public static class CreateGuild
{
    public class Query : IRequest<Response>
    {
        public GuildCreateRequestDto Arguments { get; set; }
        public AuthorizedUser AuthorizedUser { get; set; }

        public Query(GuildCreateRequestDto arguments, AuthorizedUser authorizedUser)
        {
            Arguments = arguments;
            AuthorizedUser = authorizedUser;
        }
    }

    public class Response
    {
        public Response(GuildProfileShortInfoDto guild)
        {
            Guild = guild;
        }

        public GuildProfileShortInfoDto Guild { get; set; }
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser creator = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);
            Guild userCurrentGuild = await _context.GuildMembers.ReadForStudent(creator.Id);

            var createdGuild = Guild.Create(creator, userCurrentGuild, request.Arguments);

            _context.Guilds.Add(createdGuild);
            return new Response(new GuildProfileShortInfoDto(createdGuild));
        }
    }
}