﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Endpoint.Client.Pages.Quests
{
    public partial class QuestJournalPage
    {
        private IReadOnlyList<QuestInfoResponse> _activeQuests;

        protected override async Task OnInitializedAsync()
        {
            var questControllerClient = new QuestControllerClient(await Http.TrySetHeader(LocalStorage));
            _activeQuests = await questControllerClient.GetActive();
        }

        private string LinkToQuestProfilePage(QuestInfoResponse quest) => $"/quest/profile/{quest.Id}";
        private string LinkToQuestResponsePage(QuestInfoResponse quest) => $"/quest/response/{quest.Id}";
    }
}
