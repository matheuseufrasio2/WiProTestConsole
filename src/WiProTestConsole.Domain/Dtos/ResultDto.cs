using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiProTestConsole.Domain.Dtos
{
    public class ResultDto
    {

        public string ID_MOEDA { get; set; }
        public string DATA_REF { get; set; }
        public double VL_COTACAO { get; set; }

        public ResultDto(DadosMoedaDto dadoMoeda, List<DadosCotacaoNormalDateDto> dadosCotacaoDto)
        {
            this.ID_MOEDA = dadoMoeda.ID_MOEDA;
            this.DATA_REF = dadoMoeda.DATA_REF.ToString("dd/MM/yyyy");
            this.VL_COTACAO = dadosCotacaoDto.Where(x => x.dat_cotacao == dadoMoeda.DATA_REF).FirstOrDefault().vlr_cotacao;
        }
    }
}
