using LocaleSDK.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace LocaleSDK.Middlewares
{
    /// <summary>
    /// 透傳參數設定與多語系 Middleware 擴充
    /// </summary>
    public static class LocaleMiddlewareExtension
    {
        public static void UsePassThroughAndLocale(this IApplicationBuilder app)
        {
            app.UseMiddleware<PassThroughAndLocaleMiddleware>();
        }
    }

    /// <summary>
    /// 透傳參數設定與多語系Middleware，依照header與appsettting相關區段來進行對應設定
    /// </summary>
    public class PassThroughAndLocaleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public PassThroughAndLocaleMiddleware(RequestDelegate next, IConfiguration configurationSettings)
        {
            _next = next;
            _config = configurationSettings;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var passThroughOptions = _config.GetSection("PassThroughOptions").Get<IEnumerable<PassThroughOptions>>();
            var isCurrentThreadUIBinding = _config.GetSection("IsCurrentUIBindingLocale").Get<bool>();
            if (passThroughOptions != null) PassThroughProcess(context, passThroughOptions, isCurrentThreadUIBinding);
            await _next(context);
        }


        /// <summary>
        /// 透傳參數處理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        /// <param name="isCurrentThreadUIBinding"></param>
        private void PassThroughProcess(HttpContext context, IEnumerable<PassThroughOptions> options, bool isCurrentThreadUIBinding = false)
        {
            foreach (var item in options)
            {
                var defaultValue = item.DefaultValue;
                var queryParam = context.Request.Query[item.ParamName];
                var headerParam = context.Request.Headers[item.ParamName];

                if (!String.IsNullOrEmpty(queryParam))
                {
                    defaultValue = queryParam;
                }
                else if (!String.IsNullOrEmpty(headerParam))
                {
                    defaultValue = headerParam;
                }

                if (String.IsNullOrEmpty(defaultValue)) continue;

                context.Items[item.ParamName.ToLower()] = defaultValue;

                if (item.IsPass) context.Response.Headers.Add(item.ParamName, defaultValue);
                
                if (item.ParamName.ToLower() == "locale" && isCurrentThreadUIBinding) SetCurrentUICulture(defaultValue);
            }
        }

        private void SetCurrentUICulture(string locale)
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
