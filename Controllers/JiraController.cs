using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JiraLeadTimePanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JiraLeadTimePanel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JiraController : ControllerBase
    {
        private readonly ILogger<JiraController> _logger;
        private HttpClient client;
        private byte[] cred;

        public JiraController(ILogger<JiraController> logger)
        {
            _logger = logger;
            client = new HttpClient();
            cred = UTF8Encoding.UTF8.GetBytes("crfsmeu:crfs65");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(cred));
        }

        [HttpGet]
        [Route("user/{userName}")]
        public async Task<User> GetUser(string userName)
        {
            var url = string.Concat("https://jiracorp.ctsp.prod.cloud.ihf/rest/api/2/user?username=", userName);

            var response = await client.GetAsync(url);

            return JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
        }

        [HttpGet]
        [Route("{projectName}")]
        public async Task<IEnumerable<Card>> GetCards(string projectName)
        {
            var url = string.Concat("https://jiracorp.ctsp.prod.cloud.ihf/rest/api/2/search?jql=project in (",
                                    projectName,
                                    ") AND resolution=Unresolved AND (type = Epic OR (type in (Story, Task, Refinement, Spike) AND sprint in openSprints()))&maxResults=1000");

            var response = await client.GetAsync(url);
            var project = JsonSerializer.Deserialize<Project>(await response.Content.ReadAsStringAsync());

            List<Card> cards = new List<Card>();
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

        private Card CreateCard(Issue issue, string leadtime)
        {
            Card card = new Card();

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
            var url = string.Concat("https://jiracorp.ctsp.prod.cloud.ihf/rest/agile/1.0/issue/", key, "?expand=changelog");

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
