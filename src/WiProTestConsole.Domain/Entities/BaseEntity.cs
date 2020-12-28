using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiProTest.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [NotMapped]
        public FluentValidation.Results.ValidationResult ValidationResult { get; protected set; }
    }
}
