﻿using Iwentys.Domain.Achievements;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Subcontext
{
    public interface IAchievementDbContext
    {
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<StudentAchievement> StudentAchievements { get; set; }
        public DbSet<GuildAchievement> GuildAchievements { get; set; }
    }

    public static class AchievementDbContextExtensions
    {
        public static void OnAchievementModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentAchievement>().HasKey(a => new { a.AchievementId, a.StudentId });
            modelBuilder.Entity<GuildAchievement>().HasKey(a => new { a.AchievementId, a.GuildId });
        }
    }
}