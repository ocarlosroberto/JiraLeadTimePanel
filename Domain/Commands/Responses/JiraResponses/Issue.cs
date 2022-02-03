namespace JiraLeadTimePanel.Domain.Commands.Responses.JiraResponses
{
    public class Issue
    {
        public string id { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }
    }
}