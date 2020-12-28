using FluentValidation;
using Newtonsoft.Json;
using System;

namespace WiProTest.Domain.Entities
{
    public class Moeda : BaseEntity
    {
        [JsonProperty(PropertyName = "moeda")]
        public string NomeMoeda { get; set; }
        [JsonProperty(PropertyName = "data_inicio")]
        public DateTime DataInicio { get; set; }
        [JsonProperty(PropertyName = "data_fim")]
        public DateTime DataFim { get; set; }
        public int IdLote { get; set; }
        public Lote Lote { get; set; }

        public bool Validar()
        {
            ValidationResult = new MoedaValidacao().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class MoedaValidacao : AbstractValidator<Moeda>
    {
        public MoedaValidacao()
        {
            RuleFor(m => m.NomeMoeda)
                .NotEmpty().WithMessage("O nome não pode ser vazio.")
                .NotNull().WithMessage("O campo não pode ser nulo.");
        }
    }
}
