using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERVITEC_FENIX.Models
{
    public class Pago
    {
        [Display(Name = "CÓDIGO")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "REPARACION")]
        public string Reparacion { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "FECHA")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "MODO DE PAGO")]
        public string? Modo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El importe debe ser un número válido con hasta dos decimales.")]
        [Display(Name = "IMPORTE")]
        public double Importe { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "ANULADO")]
        public string? Anulado { get; set; }
    }
}