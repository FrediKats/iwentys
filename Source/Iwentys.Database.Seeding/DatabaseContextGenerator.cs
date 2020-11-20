using System.Collections.Generic;
using Iwentys.Database.Seeding.EntityGenerators;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Seeding
{
    public class DatabaseContextGenerator
    {
        public DatabaseContextGenerator()
        {
            InitStudyTables();
        }

        public List<StudentEntity> Students { get; set; }
        public List<GuildEntity> Guilds { get; set; }
        public List<GuildMemberEntity> GuildMembers { get; set; }
        public List<GuildPinnedProjectEntity> GuildPinnedProjects { get; set; }

        public List<GuildAchievementEntity> GuildAchievementModels { get; set; }
        public List<StudentAchievementEntity> StudentAchievementModels { get; set; }

        private void InitStudyTables()
        {
            
        }
    }
}