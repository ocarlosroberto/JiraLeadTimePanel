using System.Collections.Generic;

namespace JiraLeadTimePanel.Domain.Commands.Responses.JiraResponses
{
    public class History
    {
        public string created { get; set; }
        public List<Item> items { get; set; }
    }
}