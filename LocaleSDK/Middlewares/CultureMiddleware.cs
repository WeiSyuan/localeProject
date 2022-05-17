using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LocaleSDK.Middlewares
{
    /// <summary>
    /// 多語系 Middleware 擴充
    /// </summary>
    public static class CultureMiddlewareExtension
    {
        public readonly static string[] _supportedCultures = new[] { "zh-tw", "en" };
        public static void UseCultureBindingLocale(this IApplicationBuilder app)
        {
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(_supportedCultures.First())
                .AddSupportedCultures(_supportedCultures)
                .AddSupportedUICultures(_supportedCultures);
            app.UseRequestLocalization(localizationOptions);
            app.UseMiddleware<CultureMiddleware>();
        }
    }

    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public CultureMiddleware(RequestDelegate next, IConfiguration configurationSettings)
        {
            _next = next;
            _config = configurationSettings;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var locale = String.Empty;
            var queryParam = context.Request.Query["locale"];
            var headerParam = context.Request.Headers["locale"];

            var isCurrentThreadUIBinding = _config.GetSection("IsCurrentUIBindingLocale").Get<bool>();

            if (!String.IsNullOrEmpty(queryParam))
            {
                locale = queryParam;
            }
            else if (!String.IsNullOrEmpty(headerParam))
            {
                locale = headerParam;
            }

            var isSupport = CultureMiddlewareExtension._supportedCultures.Any(p => p == locale.ToLower());

            if (!String.IsNullOrEmpty(locale) && isCurrentThreadUIBinding && isSupport) SetCurrentCulture(locale);

            await _next(context);
        }
        private void SetCurrentCulture(string locale)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);
            }
            catch (Exception)
            {

            }
        }
    }
}
