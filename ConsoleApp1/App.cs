using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class App
    {
        private readonly ILogger _logger;
        private IHttpClientFactory _httpFactory { get; set; }
        public App(ILogger<App> logger, IHttpClientFactory httpFactory)
        {
            _logger = logger;
            _httpFactory = httpFactory;
        }


        public async Task<string> Run()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;

            var tasks = new List<Task>();
            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(GetlocaleApiAsync(i, RandomGetLocale()));
            }


            _logger.LogInformation($"Task Start");
            if (tasks.Count > 0) await Task.WhenAll(tasks.ToArray());
            _logger.LogInformation($"Task End");

            return "Complete";
        }

        private static string RandomGetLocale()
        {
            var values = Enum.GetValues(typeof(LocaleEnum));
            var rnd = new Random(DateTime.Now.Millisecond);
            var value = (LocaleEnum)values.GetValue(rnd.Next(values.Length))!;
            var result = GetDescription<LocaleEnum>(value.ToString());
            return result;
        }

        private async Task<bool> GetlocaleApiAsync(int no, string locale)
        {
            return await Task.Run(async () =>
            {
                var client = _httpFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $@"https://localhost:5001/api/locale/Get/Request/Header/Locale?locale={locale}");
                var response = await client.SendAsync(request);

                var httplocale = await response.Content.ReadAsStringAsync();
                if (locale.ToLower() == httplocale.ToLower())
                {
                    _logger.LogInformation($"{no} => OK => {locale}  {httplocale}");
                }
                else
                {
                    _logger.LogError($"{no} => ERROR => {locale}  {httplocale}");
                }
                return true;
            });

        }


        static string GetDescription<T>(string value)
        {
            Type type = typeof(T);
            var name = Enum.GetNames(type)
                            .Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                            .Select(d => d)
                            .FirstOrDefault();
            if (name == null) return string.Empty;
            var field = type.GetField(name);
            var customAttribute = field!.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttribute == null || customAttribute.Length == 0) return name;

            var description = ((DescriptionAttribute)customAttribute[0]).Description;
            return description;
        }
    }
}
