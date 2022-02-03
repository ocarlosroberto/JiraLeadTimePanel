namespace JiraLeadTimePanel.Domain.Commands.Responses.JiraResponses
{
    public class Fields
    {
        public string summary { get; set; }
        public Assignee assignee { get; set; }
        public IssueType issuetype { get; set; }
        public Status status { get; set; }
        public string customfield_10001 { get; set; } // Parent
        public Parent parent { get; set; }
        public CustomField_16915 customfield_16915 { get; set; } // Story Type
        public float? customfield_16600 { get; set; } // BCP
        public int? aggregatetimespent { get; set; }
    }
}