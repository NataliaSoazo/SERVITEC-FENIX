using System.ComponentModel.DataAnnotations;
namespace SERVITEC_FENIX.Models
{
    public class Cliente
    {    [Display(Name = "CÓDIGO")]
        public int Id { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
          [Display(Name = "NOMBRE")]
        public string Nombre { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
          [Display(Name = "APELLIDO")]
        public string Apellido { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
          [Display(Name = "DOMICILIO")]
        public string Domicilio { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
          [Display(Name = "CIUDAD")]
        public string Ciudad { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
          [Display(Name = "TELÉFONO")]
        public string Telefono { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
          [Display(Name = "CORREO ELECTRÓNICO")]
        public string Correo { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
          [Display(Name = "LATITUD")]
        public string Latitud { get; set; }
         [Display(Name = "LONGITUD")]
         [Required(ErrorMessage = "Campo obligatorio")]
        public string Longitud { get; set; }

    }
}
