using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SERVITEC_FENIX.Models;

namespace SERVITEC_FENIX{
     public class OrdenReparacion{
        [Display(Name = "IDENTIFICADOR")]
        public int Id { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "CÓD DE REPARACIÓN")]
        public string CodigoReparacion { get; set; }
         [Required(ErrorMessage = "Campo obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "FECHA DE INGRESO")]
        public DateTime FechaRecepcion { get; set; }
        [Display(Name = "CLIENTE O CODIGO DE CLIENTE")]
         [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey(nameof(IdCliente))]
        public int IdCliente { get; set; }
        public Cliente DatosCliente { get; set; }


        [Display(Name = "CODIGO APARATO")]
         [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey(nameof(IdAparato))]
        public int IdAparato { get; set; }
        public Aparato DatosAparato { get; set; }


        [Display(Name = "MARCA")]
        [Required]
        [ForeignKey(nameof(IdMarca))]
        public int IdMarca { get; set; }
        public Marca DatosMarca { get; set; }
        [Required]
         [Display(Name = " FALLA")]
        public string Falla { get; set; }
        [Required]
        [Display(Name = "NRO DE SERIE")]
        public string NroSerie { get; set; }
        [Display(Name = "VALOR PRESUPUESTADO ")]
        public double Valor { get; set; }
        
       public OrdenReparacion()
    {
        // Establecer la fecha actual en el constructor
        FechaRecepcion = DateTime.Today;
    }

     }
   

}