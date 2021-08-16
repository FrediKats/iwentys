﻿using Iwentys.Modules.Gamification.Quests;
using Iwentys.Modules.Gamification.Raids;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Modules.Gamification
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGamificationModule(this IServiceCollection services)
        {
            services.AddScoped<RaidService>();
            services.AddMediatR(typeof(QuestController).Assembly);

            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(QuestController).Assembly));

            return services;
        }
    }
}
