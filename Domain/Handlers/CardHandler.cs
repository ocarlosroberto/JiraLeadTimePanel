using System;
using System.Net.Http;
using System.Threading.Tasks;
using JiraLeadTimePanel.Domain.Commands.Request;
using JiraLeadTimePanel.Domain.Commands.Responses;
using JiraLeadTimePanel.Domain.Commands.Responses.JiraResponses;
using System.Text.Json;
using System.Collections.Generic;

namespace JiraLeadTimePanel.Domain.Handlers
{
    public class CardHandler : ICardHandler
    {
        private HttpClient client;
        private readonly string baseAPI = "https://jiracorp.ctsp.prod.cloud.ihf/rest/";

        public CardHandler()
        {
            client = new HttpClient();
        }

        public async Task<IEnumerable<CardResponse>> Handle(string token, CardRequest request)
        {
            client.DefaultRequestHeaders.Add("Authorization", token);

            // var url = string.Concat(baseAPI,
            //                         "api/2/search?jql=project in (",
            //                         request.projectName,
            //                         ") AND resolution=Unresolved AND (type = Epic OR (type in (Story, Task, Refinement, Spike) AND sprint in openSprints()))&maxResults=1000");

            var url = " http://localhost:3000/card";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erro ao obter cards");

            var project = JsonSerializer.Deserialize<Project>(await response.Content.ReadAsStringAsync());

            List<CardResponse> cards = new List<CardResponse>();

            string leadtime = string.Empty;

            foreach (var issue in project.issues)
            {
                if ((issue.fields.issuetype.name == "Epic") || (issue.fields.issuetype.name == "Story"))
                {
                    System.Threading.Thread.Sleep(500);
                    leadtime = await GetLeadtime(issue.key);
                }
                else
                    leadtime = "n/a";

                var card = CreateCard(issue, leadtime);

                cards.Add(card);
            }

            return cards;
        }

        private CardResponse CreateCard(Issue issue, string leadtime)
        {
            CardResponse card = new CardResponse();

            card.key = issue.key;
            card.summary = issue.fields.summary;
            card.status = issue.fields.status.statusCategory.name;
            card.issuetype = issue.fields.issuetype.name;
            card.assignee = issue.fields.assignee != null ? issue.fields.assignee.displayName.Split(' ')[0] : "sem posse";
            card.leadtime = leadtime;
            card.parent = issue.fields.customfield_10001 != null ? issue.fields.customfield_10001 : issue.fields.parent != null ? issue.fields.parent.key : "";
            card.bcp = issue.fields.customfield_16600 != null ? issue.fields.customfield_16600.ToString() : "0";
            card.aggregatetimespent = issue.fields.aggregatetimespent != null ? (Convert.ToDecimal(issue.fields.aggregatetimespent) / 3600).ToString("0.00") : "0";
            card.storyType = issue.fields.customfield_16915 != null ? issue.fields.customfield_16915.value : "";
            card.bcpXhours = card.bcp == "0" || card.aggregatetimespent == "0" ? "n/a" : (Convert.ToDecimal(card.bcp) / Convert.ToDecimal(card.aggregatetimespent)).ToString("0.0");

            return card;
        }

        private async Task<string> GetLeadtime(string key)
        {
            var url = string.Concat(baseAPI, "agile/1.0/issue/", key, "?expand=changelog");

            var response = await client.GetAsync(url);

            while (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                System.Threading.Thread.Sleep(3000);
                response = await client.GetAsync(url);
            }

            var expand = JsonSerializer.Deserialize<Expand>(await response.Content.ReadAsStringAsync());

            foreach (var history in expand.changelog.histories)
            {
                foreach (var item in history.items)
                {
                    if (item.field == "Epic Child")
                        return DateTime.Now.Subtract(Convert.ToDateTime(history.created)).Days.ToString();
                    if (item.field == "status")
                        return DateTime.Now.Subtract(Convert.ToDateTime(history.created)).Days.ToString();
                }
            }

            return "n/a";
        }
    }
}