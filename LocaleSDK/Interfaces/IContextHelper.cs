using Microsoft.AspNetCore.Http;

namespace LocaleSDK.Interfaces
{
    public interface IContextHelper
    {
        HttpContext GetContext();
    }
}