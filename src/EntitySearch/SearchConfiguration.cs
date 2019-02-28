using System;
using System.Collections.Generic;
using System.Text;

namespace EntitySearch
{
    public class SearchConfiguration
    {
        public int? TokenMinimumSize { get; set; }
        public int? TokenMaximumSize { get; set; }
        public List<char> SupressCharacters { get; set; }
        public List<string> SupressTokens { get; set; }
        public SearchConfiguration()
        {
            TokenMinimumSize = 3;
            SupressCharacters = new List<char> { };
            SupressTokens = new List<string> { };
        }
    }
}
