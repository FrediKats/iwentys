using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities.Newsfeeds;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Models.Transferable.Study
{
    public class SubjectProfileResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public List<NewsfeedInfoResponse> Newsfeeds { get; set; }

        public static SubjectProfileResponse Wrap(SubjectEntity entity)
        {
            return Wrap(entity, new List<SubjectNewsfeedEntity>());
        }

        public static SubjectProfileResponse Wrap(SubjectEntity entity, List<SubjectNewsfeedEntity> newsfeeds)
        {
            return new SubjectProfileResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Newsfeeds = newsfeeds.SelectToList(s => NewsfeedInfoResponse.Wrap(s.Newsfeed))
            };
        }
    }
}