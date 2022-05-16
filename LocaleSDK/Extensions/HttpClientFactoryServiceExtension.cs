using LocaleSDK.Interfaces;
using LocaleSDK.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            var contextHelper = services.BuildServiceProvider().GetService<IContextHelper>();
            if (contextHelper == null || config == null) return;

            var passThroughOptions = config.GetSection("PassThroughOptions").Get<IEnumerable<PassThroughOptions>>();
            if (passThroughOptions == null) return;

            //依據AppSettings判斷哪些參數要透傳,預設名稱為customer
            services.AddHttpClient("customer", client =>
            {
                foreach (var item in passThroughOptions)
                {
                    if (!item.IsPass) continue;
                    client.DefaultRequestHeaders.Add(item.ParamName, contextHelper.GetContextItem<string>(item.ParamName));
                }
            });
        }


    }
}
