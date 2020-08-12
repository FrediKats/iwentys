using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.GoogleTableParsing;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Core.Daemons
{
    public class MarkUpdateDaemon : DaemonWorker
    {
        private readonly GoogleTableUpdateService _googleTableUpdateService;
        private readonly ISubjectForGroupRepository _subjectForGroupRepository;

        public MarkUpdateDaemon(TimeSpan checkInterval, GoogleTableUpdateService googleTableUpdateService, ISubjectForGroupRepository subjectForGroupRepository) : base(checkInterval)
        {
            _googleTableUpdateService = googleTableUpdateService;
            _subjectForGroupRepository = subjectForGroupRepository;
        }

        protected override void Execute()
        {
            List<SubjectForGroup> groups = _subjectForGroupRepository.Read().ToList();
            groups.ForEach(g => _googleTableUpdateService.UpdateSubjectActivityForGroup(g));
        }
    }
}