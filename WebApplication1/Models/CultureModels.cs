using System.Threading;

namespace WebApplication1.Models
{
    public static class CultureModels
    {
        public static string locale
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.Name;
            }
        }
    }
}
