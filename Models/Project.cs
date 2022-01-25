using System.Collections.Generic;

namespace JiraLeadTimePanel.Models
{
    public class Project
    {
        public int total { get; set; }
        public List<Issue> issues { get; set; }
    }

    public class Issue
    {
        public string id { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }
    }

    public class Fields
    {
        public string summary { get; set; }
        public Assignee assignee { get; set; }
        public IssueType issuetype { get; set; }
        public Status status { get; set; }
        public string customfield_10001 { get; set; } // Parent
        public Parent parent { get; set; }
        public CustomField_16195 customfield_16195 { get; set; } // Story Type
        public float? customfield_16600 { get; set; } // BCP
        public int? aggregatetimespent { get; set; }
    }

    public class IssueType
    {
        public string name { get; set; }
    }

    public class CustomField_16195
    {
        public string value { get; set; }
    }

    public class Parent
    {
        public string key { get; set; }
    }

    public class Status
    {
        public StatusCategory statusCategory { get; set; }
    }

    public class StatusCategory
    {
        public string name { get; set; }
    }

    public class Assignee
    {
        public string name { get; set; }
        public string displayName { get; set; }
    }
}