using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SERVITEC_FENIX.Models;

namespace SERVITEC_FENIX{
     public class Reparacion{
        [Display(Name = "ID REPARACION")]
        public int Id { get; set; }
        [Display(Name = "COD DE ORDEN REPARACION")]
        public int IdOrden { get; set; }
        public OrdenReparacion DatosOrden { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "FECHA DE REPARACION")]
        public DateTime FechaReparacion { get; set; }
        [Display(Name = " DETALLE DE REPARACION")]
        public string Detalle { get; set; }
         [DataType(DataType.Date)]
        [Display(Name = "FECHA DE ENTREGA")]
        public DateTime FechaEntrega { get; set; }
        [Display(Name = "PAGO")]
       
        public double Pago { get; set; }
        
        public Reparacion()
    {
        // Establecer la fecha actual en el constructor
        FechaReparacion = DateTime.Today;
        
    }

     }
   

}