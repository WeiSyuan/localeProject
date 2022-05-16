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

        /// <summary>
        /// 取得Context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpContext GetContext()
        {
            return _accessor.HttpContext;
        }

        /// <summary>
        /// 取得對應的ContextItem值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetContextItem<T>(string name)
        {
            _accessor.HttpContext.Items.TryGetValue(name.ToLower(), out object itemValue);
            return (T)itemValue;
        }
    }
}
