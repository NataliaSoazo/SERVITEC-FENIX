using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SERVITEC_FENIX.Models;


namespace SERVITEC_FENIX.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public PagoController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
   [Authorize]
    public IActionResult Index()
    {  
        RepositorioPago rp = new RepositorioPago();
        IList<Pago> lista = new List<Pago>();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        try
        {
            lista = rp.GetPagos();

            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            else if (TempData.ContainsKey("Error"))
            {
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }
         catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de pagos");
            TempData["Error"] = "Ocurrio un error al obtener la lista de pagos";
            ViewBag.Error = TempData["Error"];
            return View(lista);
        }
    }
    [HttpGet("index/filtrar")]
    
public IActionResult Index(DateTime? fechaInicio, DateTime? fechaFin) 
{  
    RepositorioPago rp = new RepositorioPago();
    IList<Pago> lista = rp.GetPagos(); // Obtiene todos los pagos
    var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
    ViewBag.UserRole = userRole;

    try
    {
        // Filtrar por rango de fechas si están presentes
        if (fechaInicio.HasValue || fechaFin.HasValue)
        {
            if (fechaInicio.HasValue)
            {
                lista = lista.Where(p => p.Fecha >= fechaInicio.Value).ToList();
            }

            if (fechaFin.HasValue)
            {
                lista = lista.Where(p => p.Fecha <= fechaFin.Value).ToList();
            }

            // Calcular el total recibido solo si hay filtrado
            ViewBag.TotalRecibido = lista.Sum(p => p.Importe);
        }
        else
        {
            // No asignar total si no hay filtrado
            ViewBag.TotalRecibido = null; // O puedes omitir esta línea
        }

        // Mensajes para la vista
        if (TempData.ContainsKey("Mensaje"))
        {
            ViewBag.Mensaje = TempData["Mensaje"];
        }
        else if (TempData.ContainsKey("Error"))
        {
            ViewBag.Error = TempData["Error"];
        }
        
        return View(lista);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al obtener la lista de pagos");
        TempData["Error"] = "Ocurrió un error al obtener la lista de pagos";
        ViewBag.Error = TempData["Error"];
        return View(lista);
    }
}
   // [Authorize]
    public IActionResult Editar(int? id)
    {   List<string> Referencia = new List<string>{ "EFECTIVO","TRANSF. BANCARIA", "OTRO"};
        ViewBag.Modo = Referencia;
         RepositorioReparacion repo = new RepositorioReparacion();
         ViewBag.Reparaciones = repo.ObtenerReparaciones();
        var pago = new Pago();

        if (id.HasValue && id.Value > 0)
        {
            RepositorioPago rp = new RepositorioPago();
            pago = rp.GetPago(id.Value) ?? new Pago();
        }

        return View(pago);
    }

    
    //[Authorize]
    public IActionResult Guardar(Pago pago)
    {  
      /*  try
        {*/
             RepositorioPago rp = new RepositorioPago();

            if (pago.Id > 0)
            {
                rp.ModificarPago(pago);
                   return RedirectToAction(nameof(Index));
            }
            else
            {
                rp.AltaPago(pago);
                 return RedirectToAction(nameof(Index));
            }
        /*}
        catch (System.Exception)
        {
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));

        }*/
    }
    //[Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id) //Es un anulado logico
    {
        try
        {
            RepositorioPago rp = new RepositorioPago();
            RepositorioUsuario ru = new RepositorioUsuario();
            var usuario = ru.ObtenerPorEmail(User.Identity.Name);
            rp.EliminarPago(id);
            TempData["Mensaje"] = "El pago ha sido anulado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));
        }
    }
    //[Authorize]
    public IActionResult Detalles(int id)
    {
        RepositorioPago rp = new RepositorioPago();
        RepositorioUsuario ru = new RepositorioUsuario();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        var p = rp.GetPago(id);
        return View(p);
    }
    //[Authorize]   
     


}
