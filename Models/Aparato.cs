
using System.ComponentModel.DataAnnotations;
namespace SERVITEC_FENIX.Models
{
    public class Aparato
    {   [Display(Name = "CÓDIGO")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "APARATO")]
        public string NombreA { get; set; }
        
    }
}