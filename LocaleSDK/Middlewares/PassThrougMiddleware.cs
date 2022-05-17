using LocaleSDK.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LocaleSDK.Middlewares
{
    /// <summary>
    /// 透傳參數設定 Middleware 擴充
    /// </summary>
    public static class PassThroughMiddlewareExtension
    {
        public static void UsePassThrough(this IApplicationBuilder app)
        {
            app.UseMiddleware<PassThrougMiddleware>();
        }
    }

    /// <summary>
    /// 透傳參數設定，依照header與appsettting相關區段來進行對應設定
    /// </summary>
    public class PassThrougMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public PassThrougMiddleware(RequestDelegate next, IConfiguration configurationSettings)
        {
            _next = next;
            _config = configurationSettings;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var passThroughOptions = _config.GetSection("PassThroughOptions").Get<IEnumerable<PassThroughOptions>>();
            if (passThroughOptions != null) PassThroughProcess(context, passThroughOptions);
            await _next(context);
        }


        /// <summary>
        /// 透傳參數處理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        /// <param name="isCurrentThreadUIBinding"></param>
        private void PassThroughProcess(HttpContext context, IEnumerable<PassThroughOptions> options)
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
                if (item.IsPass) context.Response.Headers.TryAdd(item.ParamName, defaultValue);
            }
        }


    }
}
