using LocaleSDK.Helpers;
using LocaleSDK.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LocaleSDK.Extensions
{
    public static class ContextServiceExtension
    {
        public static void AddContextHelper(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddHttpContextAccessor();
            services.AddScoped<IContextHelper, ContextHelper>();
        }
    }
}
