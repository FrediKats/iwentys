﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Flurl.Http;
using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Tributes.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Guilds
{
    public class GuildTributeControllerClient
    {
        public GuildTributeControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<TributeInfoResponse> Get(int tributeId)
        {
            return new FlurlClient(Client)
                .Request($"/api/guild/tribute/{tributeId}")
                .GetJsonAsync<TributeInfoResponse>();
        }

        public Task<List<TributeInfoResponse>> GetGuildTribute(int guildId)
        {
            return Client.GetFromJsonAsync<List<TributeInfoResponse>>($"/api/guild/tribute/get-for-guild?guildId={guildId}");
        }

        public Task<TributeInfoResponse> FindStudentActiveTribute()
        {
            return Client.FindFromJsonAsync<TributeInfoResponse>($"/api/guild/tribute/get-for-student/active");
        }

        public async Task CompleteTribute(TributeCompleteRequest tributeCompleteRequest)
        {
            await Client.PutAsJsonAsync($"/api/guild/tribute/complete", tributeCompleteRequest);
        }

        public async Task CancelTribute(long tributeId)
        {
            await Client.PutAsJsonAsync($"/api/guild/tribute/cancel", tributeId);
        }
    }
}