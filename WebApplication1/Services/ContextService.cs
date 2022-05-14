using LocaleSDK.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WebApplication1.Services
{
    public class ContextService
    {
        private IContextHelper _contextHelper;
        public ContextService(IContextHelper contextHelper)
        {
            _contextHelper = contextHelper;
        }


        public bool SetResponseHeader(IDictionary<string, string> headers)
        {
            var context = _contextHelper.GetContext();

            foreach (var item in headers)
            {
                if (context.Response.Headers.TryGetValue(item.Key, out _))
                {
                    context.Response.Headers[item.Key] = item.Value;
                }
                else
                {
                    context.Response.Headers.Add(item.Key, item.Value);
                }
            }
            return true;
        }

        public string GetRquestHeaderLocale()
        {
            var context = _contextHelper.GetContext();

            _ = context.Request.Headers.TryGetValue("locale", out var locale);

            if (!String.IsNullOrEmpty(locale)) return locale;

            _ = context.Request.Query.TryGetValue("locale", out locale);

            return locale;
        }
    }
}
