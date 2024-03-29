﻿using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public class UnblockStudent
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser User { get; set; }
        public int GuildId { get; set; }
        public int MemberId { get; set; }

        public Query(AuthorizedUser user, int guildId, int memberId)
        {
            User = user;
            GuildId = guildId;
            MemberId = memberId;
        }
    }

    public class Response
    {

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
            IwentysUser student = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id);
            Guild guild = await _context.Guilds.GetById(request.GuildId);
            IwentysUser iwentysUser = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.MemberId);
            GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _context.GuildLastLeaves);

            GuildMember member = guild.Members.Find(m => m.MemberId == request.MemberId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! AuthorId: {request.MemberId} GuildId: {request.GuildId}");

            guild.RemoveMember(student, iwentysUser, guildLastLeave);
            return new Response();
        }
    }
}