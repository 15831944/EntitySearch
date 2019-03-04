using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EntitySearch
{
    public static class Middleware
    {
        public static IServiceCollection AddEntitySearch(this IServiceCollection services)
        {
            SearchConfiguration.GetSearchConfiguration();

            services.AddSingleton<SearchConfiguration>();

            return services;
        }
        public static IServiceCollection SetTokenMinimumSize(this IServiceCollection services, int? tokenMinimumSize = null)
        {
            SearchConfiguration.GetSearchConfiguration().TokenMinimumSize = tokenMinimumSize;

            return services;
        }
        public static IServiceCollection SetTokenMaximumSize(this IServiceCollection services, int? tokenMinimumSize = null)
        {
            SearchConfiguration.GetSearchConfiguration().TokenMaximumSize = tokenMinimumSize;

            return services;
        }
        public static IServiceCollection SetSupressCharacters(this IServiceCollection services, IList<char> supressCharacters = null)
        {
            SearchConfiguration.GetSearchConfiguration().SupressCharacters = supressCharacters;

            return services;
        }
        public static IServiceCollection SetSupressTokens(this IServiceCollection services, IList<string> supressTokens = null)
        {
            SearchConfiguration.GetSearchConfiguration().SupressTokens = supressTokens;

            return services;
        }
    }
}
