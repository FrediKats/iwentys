﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;
using MudBlazor;

namespace Iwentys.Endpoints.WebClient.Modules.AccountManagement.MentorPages
{
    public partial class MentorsJournal
    {
        private ICollection<SubjectMentorsDto> _subjectsMentors;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _subjectsMentors = await MentorsManagementClient.GetAllAsync();
        }

        public void RemoveMentor(MudChip chip)
        {
            
        }
    }
}