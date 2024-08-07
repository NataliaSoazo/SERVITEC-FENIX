
using System.ComponentModel.DataAnnotations;
namespace SERVITEC_FENIX.Models
{
    public class Marca
    {   [Display(Name = "CÓDIGO")]
        public int Id { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "MARCA")]
        public string NombreM { get; set; }
        
    }
}