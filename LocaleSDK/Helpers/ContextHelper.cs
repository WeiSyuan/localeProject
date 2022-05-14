using LocaleSDK.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocaleSDK.Helpers
{
    public class ContextHelper : IContextHelper
    {
        private IHttpContextAccessor _accessor;

        public ContextHelper(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        public HttpContext GetContext()
        {
            return _accessor.HttpContext;
        }
    }
}
