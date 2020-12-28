using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WiProTestConsole.App;
using WiProTestConsole.App.Interfaces;
using WiProTestConsole.Domain.Interfaces.Services;
using WiProTestConsole.Domain.Services;
using WiProTestConsole.Domain.Utilities;

namespace WiProTestConsole.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = MainAsync(args);
            t.Wait();

        }

        static async Task MainAsync(string[] args)
        {
            while (true)
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                var appProvider = serviceCollection.BuildServiceProvider();

                var mainApp = appProvider.GetService<IMoedaApp>();
                Console.WriteLine();
                Console.WriteLine("INÍCIO DO PROCESSO");
                var t = mainApp.RecuperarItemApi();
                t.Wait();

                Console.WriteLine();
                Console.WriteLine("PROCESSO FINALIZADO, o processo recomeçará em 2 minutos.");
                Thread.Sleep(60000);
                Console.WriteLine("Falta 1 minuto para recomeçar.");
                Thread.Sleep(30000);
                Console.WriteLine("Faltam 30 segundos para recomeçar.");
                Thread.Sleep(15000);
                Console.WriteLine("Faltam 15 segundos para recomeçar.");
                Thread.Sleep(5000);
                Console.WriteLine("Faltam 10 segundos para recomeçar.");
                Thread.Sleep(5000);
                Console.WriteLine("Faltam 5 segundos para recomeçar.");
                Thread.Sleep(5000);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMoedaApp, MoedaApp>();
            services.AddScoped<IMoedaService, MoedaService>();
        }
    }
}
