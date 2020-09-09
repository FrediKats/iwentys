using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.GoogleTableParsing;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Microsoft.Extensions.Logging;

namespace Iwentys.Core.Daemons
{
    public class MarkUpdateDaemon
    {
        private readonly GoogleTableUpdateService _googleTableUpdateService;
        private readonly ISubjectForGroupRepository _subjectForGroupRepository;
        private readonly ILogger _logger;

        public MarkUpdateDaemon(GoogleTableUpdateService googleTableUpdateService, ISubjectForGroupRepository subjectForGroupRepository, ILogger logger)
        {
            _googleTableUpdateService = googleTableUpdateService;
            _subjectForGroupRepository = subjectForGroupRepository;
            _logger = logger;
        }

        public void Execute()
        {
            try
            {
                List<SubjectForGroup> groups = _subjectForGroupRepository.Read().ToList();
                groups.ForEach(g => _googleTableUpdateService.UpdateSubjectActivityForGroup(g));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mark update failed");
            }
        }
    }
}