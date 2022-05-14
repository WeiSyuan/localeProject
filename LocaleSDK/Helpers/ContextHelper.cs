using LocaleSDK.Interfaces;
using Microsoft.AspNetCore.Http;

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
