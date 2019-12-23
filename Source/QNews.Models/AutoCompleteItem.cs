using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNews.Models
{
    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AutoCompleteItem 
    {
        public string query { get; set; }
        public List<string> suggestions { get; set; }
        public List<string> data { get; set; }
    }

    public class AutoCompleteResponse
    {
        public string query { get; set; }

        public List<SuggestionItem> suggestions { get; set; }

        public AutoCompleteResponse()
        {
            suggestions = new List<SuggestionItem>();
        }
    }

    public class SuggestionItem
    {
        public string value { get; set; }
        public SuggestionData data { get; set; }
    }

    public class SuggestionData
    {
        public string text { get; set; }
        public string brand { get; set; }

        public string price { get; set; }


        public string image { get; set; }
    }
}
