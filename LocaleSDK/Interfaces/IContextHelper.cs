using Microsoft.AspNetCore.Http;

namespace LocaleSDK.Interfaces
{
    public interface IContextHelper
    {
        /// <summary>
        /// 取得Context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        HttpContext GetContext();
        /// <summary>
        /// 取得對應的ContextItem值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetContextItem<T>(string name);
    }
}