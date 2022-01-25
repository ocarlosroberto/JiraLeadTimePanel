using System.Collections.Generic;

namespace JiraLeadTimePanel.Models
{
    public class Expand
    {
        public string id { get; set; }
        public string key { get; set; }
        public Changelog changelog { get; set; }
    }

    public class Changelog
    {
        public int total { get; set; }
        public List<History> histories { get; set; }
    }

    public class History
    {
        public string created { get; set; }
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string field { get; set; }
        public string fieldtype { get; set; }
        public string from { get; set; }
        public string fromString { get; set; }
        public string to { get; set; }
        public string toString { get; set; }
    }
}