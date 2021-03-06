﻿using System.Collections.Generic;
using System.Linq;

namespace EntitySearch
{
    public class SearchConfiguration
    {
        private static SearchConfiguration Instance { get; set; }
        public int? TokenMinimumSize { get; internal set; }
        public int? TokenMaximumSize { get; internal set; }
        public IList<char> SupressCharacters { get; internal set; }
        public IList<string> SupressTokens { get; internal set; }
        public SearchConfiguration()
        {
            TokenMinimumSize = 3;
            SupressCharacters = new List<char> { };
            SupressTokens = new List<string> { };
        }

        public static SearchConfiguration GetSearchConfiguration()
        {
            if (Instance == null)
                Instance = new SearchConfiguration();

            return Instance;
        }

        internal IList<string> ValidateToken(IList<string> tokens)
        {
            if (tokens == null || tokens.Count == 0)
                return tokens;

            return tokens.Where(token => 
                (!TokenMinimumSize.HasValue || token.Length >= TokenMinimumSize)
                && (!TokenMaximumSize.HasValue || token.Length <= TokenMaximumSize)
            ).ToList();
        }
        internal string ValidateSupressCharacters(string query)
        {
            if (SupressCharacters == null || SupressCharacters.Count == 0)
                return query;

            SupressCharacters.ToList().ForEach(character => query = query.Replace(character, ' '));

            return query;
        }
        internal IList<string> ValidateSupressTokens(IList<string> tokens)
        {
            if (SupressTokens == null || SupressTokens.Count == 0)
                return tokens;

            if (tokens == null || tokens.Count == 0)
                return tokens;

            SupressTokens.ToList().ForEach(token => tokens.Remove(token));

            return tokens;
        }
    }
}
