using System;
using System.Collections.Generic;
using System.Text;
using WiProTest.Domain.Entities;
using WiProTestConsole.Domain.Interfaces.Services;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using WiProTestConsole.Domain.Dtos;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;
using WiProTestConsole.Domain.Utilities;
using CsvHelper.TypeConversion;
using System.Threading.Tasks;

namespace WiProTestConsole.Domain.Services
{
    public class MoedaService : IMoedaService
    {
        public async Task GerarNovoArquivo()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44361/");

                var responseTask = await client.GetAsync("get-item-fila");

                string responseBody = await responseTask.Content.ReadAsStringAsync();

                List<MoedaDto> moedasJson = JsonConvert.DeserializeObject<List<MoedaDto>>(responseBody);



                if (!(moedasJson is null))
                {
                    foreach (var item in moedasJson)
                    {
                        List<DadosMoedaDto> dadosMoedaDto =  GetItensRangeDadosMoeda(item);
                        List<ResultDto> resultado = new List<ResultDto>();

                        if (dadosMoedaDto != null)
                        {
                            foreach (var dadoMoeda in dadosMoedaDto)
                            {
                                if (DeParaMoedaUtil.DeParaMoeda.ContainsKey(dadoMoeda.ID_MOEDA))
                                {
                                    var codigoMoeda = DeParaMoedaUtil.DeParaMoeda[dadoMoeda.ID_MOEDA];
                                    List<DadosCotacaoNormalDateDto> dadosCotacaoDto = GetDadosCotacaoDto(codigoMoeda);
                                    ResultDto resultDto = new ResultDto(dadoMoeda, dadosCotacaoDto);
                                    resultado.Add(resultDto);
                                }
                            }
                            GerarResultCsv(resultado);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }



                Console.WriteLine();
            }
        }

        private void GerarResultCsv(List<ResultDto> resultado)
        {
            string diretorioArquivos = GetDiretorioArquivos();
            using (var writer = new StreamWriter(Path.Combine(diretorioArquivos, $"Result_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.csv")))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = ";";

                    csv.WriteRecords(resultado);
                }
            }
        }

        private void GerarLinhaResultCsv(DadosMoedaDto dadoMoeda, List<DadosCotacaoNormalDateDto> dadosCotacaoDto)
        {
            string diretorioArquivos = GetDiretorioArquivos();
            using (var writer = new StreamWriter(Path.Combine(diretorioArquivos, $"Result_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.csv")))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = ";";

                    //var records
                }
            }
        }

        private List<DadosCotacaoNormalDateDto> GetDadosCotacaoDto(int codigoMoeda)
        {
            List<DadosCotacaoStringDateDto> listaDefinitivaStringDate = new List<DadosCotacaoStringDateDto>();
            List<DadosCotacaoNormalDateDto> listaDefinitivaNormalDate = new List<DadosCotacaoNormalDateDto>();
            string diretorioArquivos = GetDiretorioArquivos();
            using (var reader = new StreamReader(Path.Combine(diretorioArquivos, "DadosCotacao.csv")))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = ";";

                    var dadosCotacao = csv.GetRecords<DadosCotacaoStringDateDto>()
                        .Where(x => x.cod_cotacao == codigoMoeda).ToList();

                    foreach (var dado in dadosCotacao)
                    {
                        DadosCotacaoNormalDateDto dadoNormal = new DadosCotacaoNormalDateDto();
                        dadoNormal.dat_cotacao = Convert.ToDateTime(dado.dat_cotacao);
                        dadoNormal.cod_cotacao = dado.cod_cotacao;
                        dadoNormal.vlr_cotacao = dado.vlr_cotacao;
                        listaDefinitivaNormalDate.Add(dadoNormal);
                    }

                    return listaDefinitivaNormalDate;
                }
            }
        }

        private string GetDiretorioArquivos()
        {
            string diretorioSolution = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName).FullName).FullName;

            return Path.Combine(diretorioSolution, "assets");
        }

        private List<DadosMoedaDto> GetItensRangeDadosMoeda(MoedaDto moeda)
        {
            List<DadosMoedaDto> listaDefinitiva = new List<DadosMoedaDto>();
            string diretorioArquivos = GetDiretorioArquivos();
            using (var reader = new StreamReader(Path.Combine(diretorioArquivos, "DadosMoeda.csv")))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = ";";
                    var dadosMoedas = csv.GetRecords<DadosMoedaDto>().ToList();

                    foreach (var dadoMoeda in dadosMoedas)
                    {
                        if ((dadoMoeda.DATA_REF.CompareTo(moeda.data_inicio) > 0) && (dadoMoeda.DATA_REF.CompareTo(moeda.data_fim) < 0))
                        {
                            listaDefinitiva.Add(dadoMoeda);
                        }
                    }

                    return listaDefinitiva;
                }
            }

        }
    }
}
