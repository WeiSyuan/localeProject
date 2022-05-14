using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace LocaleSDK.Middlewares
{
    /// <summary>
    /// 多語系 Middleware 擴充
    /// </summary>
    public static class LocaleMiddlewareExtension
    {
        public static void UseLocale(this IApplicationBuilder app)
        {
            app.UseMiddleware<LocaleMiddleware>();
        }
    }

    /// <summary>
    /// 多語系 Middleware，依照 header accept-language 來設定對應語系檔
    /// </summary>
    public class LocaleMiddleware
    {
        private readonly RequestDelegate _next;

        public LocaleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var locale = "zh-TW";
            var headerLocale = context.Request.Headers["locale"];
            var queryLocale = context.Request.Query["locale"];

            if (!String.IsNullOrEmpty(headerLocale))
            {
                locale = headerLocale;
            }
            else if (!String.IsNullOrEmpty(queryLocale))
            {
                locale = queryLocale;
            }

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);
            }
            catch (Exception)
            {

            }

            context.Response.Headers.Add("locale", locale);

            await _next(context);
        }
    }
}
