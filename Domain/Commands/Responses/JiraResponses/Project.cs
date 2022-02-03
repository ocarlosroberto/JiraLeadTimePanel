using System.Collections.Generic;

namespace JiraLeadTimePanel.Domain.Commands.Responses.JiraResponses
{    public class Project
    {
        public int total { get; set; }
        public List<Issue> issues { get; set; }
    }
}