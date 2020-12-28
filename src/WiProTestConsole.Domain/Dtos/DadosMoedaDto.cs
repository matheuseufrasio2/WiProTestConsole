using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WiProTestConsole.Domain.Dtos
{
    public class DadosMoedaDto
    {
        public string ID_MOEDA { get; set; }
        public DateTime DATA_REF { get; set; }
    }
}
