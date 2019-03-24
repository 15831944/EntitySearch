using System.Collections.Generic;
using System.Linq;

namespace EntitySearch.Utilities
{
    internal static class TokenHelper
    {
        internal static IList<string> GetTokens(string query, bool queryPhrase)
        {
            query = query.ToLower();

            query = SearchConfiguration.GetSearchConfiguration().ValidateSupressCharacters(query);

            IList<string> tokens = queryPhrase ? new List<string> { query } : query.Split(" ").ToList();

            tokens = SearchConfiguration.GetSearchConfiguration().ValidateToken(tokens);

            tokens = SearchConfiguration.GetSearchConfiguration().ValidateSupressTokens(tokens);

            return tokens;
        }
    }
}
