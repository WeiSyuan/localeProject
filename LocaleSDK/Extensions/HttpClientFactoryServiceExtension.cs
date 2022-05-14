using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Http;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading;

namespace LocaleSDK.Extensions
{
    public static class HttpClientFactoryServiceExtension
    {
        public static void AddHttpClientLocaleHelper(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddHttpContextAccessor();
            services.AddHttpClient("locale", LocaleClientSetting);
        }

        private static void LocaleClientSetting(HttpClient httpclient)
        {
            httpclient.DefaultRequestHeaders.Add("locale", Thread.CurrentThread.CurrentCulture.Name);
        }
    }
}
