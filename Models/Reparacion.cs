using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SERVITEC_FENIX.Models;

namespace SERVITEC_FENIX{
     public class Reparacion{
        [Display(Name = "ID REPARACION")]
        public int Id { get; set; }
        [Display(Name = "O. REPARACION")]
        public string Codigo { get; set; }
        public OrdenReparacion DatosOrden { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "FECHA DE REPARACION")]
        public DateTime FechaReparacion { get; set; }
         [Required]
        [Display(Name = " DETALLE DE REPARACION")]
        public string? Detalle { get; set; }
         [DataType(DataType.Date)]
         [Required]
        [Display(Name = "FECHA DE ENTREGA")]
        public DateTime? FechaEntrega { get; set; }
       
        public Reparacion()
    {
        // Establecer la fecha actual en el constructor
        FechaReparacion = DateTime.Today;
        
    }

     }
   

}