namespace JiraLeadTimePanel.Models
{
    public class Card
    {
        public string key { get; set; }
        public string summary { get; set; }
        public string status { get; set; }
        public string issuetype { get; set; }
        public string assignee { get; set; }
        public string leadtime { get; set; }
        public string parent { get; set; }
        public string bcp { get; set; }
        public string aggregatetimespent { get; set; }
        public string bcpXhours { get; set; }
        public string storyType { get; set; }
        public string observations { get; set; }
    }
}