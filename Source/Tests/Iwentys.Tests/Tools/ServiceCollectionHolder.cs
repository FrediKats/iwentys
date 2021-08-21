﻿using System;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Configuration;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.AccountManagement;
using Iwentys.Modules.Gamification;
using Iwentys.Modules.Guilds;
using Iwentys.Modules.PeerReview;
using Iwentys.Modules.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Tests.Tools
{
    public class ServiceCollectionHolder
    {
        public ServiceProvider ServiceProvider { get; }

        public ServiceCollectionHolder()
        {
            IServiceCollection services = new ServiceCollection()
                .AddDbContext<IwentysDbContext>(options => options
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .UseLazyLoadingProxies())
                .AddIwentysSeeder()
                .AddIwentysMediatorHandlers()
                .AddIwentysServices()
                .AddAutoMapperConfig()
                .AddAccountManagementModule()
                .AddGamificationModule()
                .AddGuildModule()
                .AddPeerReviewModule()
                .AddStudyModule();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}