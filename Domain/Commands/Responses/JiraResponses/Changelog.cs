using System.Collections.Generic;

namespace JiraLeadTimePanel.Domain.Commands.Responses.JiraResponses
{
    public class Changelog
    {
        public int total { get; set; }
        public List<History> histories { get; set; }
    }
}