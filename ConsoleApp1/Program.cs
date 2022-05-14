// See https://aka.ms/new-console-template for more information
using ConsoleApp1;
using LocaleSDK.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = new HostBuilder()
.ConfigureServices((hostContext, services) =>
{
    services.AddHttpClientLocaleHelper();
    services.AddTransient<App>();
})
.ConfigureAppConfiguration((context, config) =>
{
    config.AddEnvironmentVariables();
    config.AddJsonFile("appsettings.json", true, true);
    config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true);
    if (args != null)
        config.AddCommandLine(args);
})
.ConfigureLogging((context, config) =>
{
    config.AddConfiguration(context.Configuration.GetSection("Logging"));
    config.AddConsole();
})
.UseConsoleLifetime();

var host = builder.Build();

using (var serviceScope = host.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    try
    {
        Console.WriteLine("AppGo");
        var myApp = services.GetRequiredService<App>();
        var result = await myApp.Run();

        Console.WriteLine(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
    Console.ReadKey();
}

return 0;

