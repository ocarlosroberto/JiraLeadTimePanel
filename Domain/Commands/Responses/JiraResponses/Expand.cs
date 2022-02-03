namespace JiraLeadTimePanel.Domain.Commands.Responses.JiraResponses
{
    public class Expand
    {
        public string id { get; set; }
        public string key { get; set; }
        public Changelog changelog { get; set; }
    }
}