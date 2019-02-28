using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EntitySearch
{
    public static class Middleware
    {
        public static IServiceCollection AddEntitySearch(this IServiceCollection services)
        {
            services.AddSingleton<SearchConfiguration>();

            return services;
        }

        public static IServiceCollection SetTokenMinimumSize(this IServiceCollection services, int? tokenMinimumSize = null)
        {
            //HttpContext.RequestServices.GetService<SearchConfiguration>().TokenMinimumSize = tokenMinimumSize;
            
            return services;
        }
    }
}
