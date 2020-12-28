using System;
using System.Threading.Tasks;
using WiProTestConsole.App.Interfaces;
using WiProTestConsole.Domain.Interfaces.Services;

namespace WiProTestConsole.App
{
    public class MoedaApp : IMoedaApp
    {
        private readonly IMoedaService moedaService;
        public MoedaApp(IMoedaService _moedaService)
        {
            moedaService = _moedaService;
        }

        public async Task RecuperarItemApi()
        {
            var a = moedaService.GerarNovoArquivo();
            a.Wait();
        }
    }
}
