using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SERVITEC_FENIX.Models;


namespace SERVITEC_FENIX.Controllers;

public class GastoController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public GastoController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult Index()
    {

        RepositorioGasto rp = new RepositorioGasto();
        IList<Gasto> lista = new List<Gasto>();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        try
        {
            lista = rp.GetGastos();

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
    [HttpGet("index/buscar")]
     [Authorize]
    public IActionResult Index(DateTime? fechaInicio, DateTime? fechaFin)
    {
        RepositorioGasto rp = new RepositorioGasto();
        IList<Gasto> lista = rp.GetGastos(); // Obtiene todos los pagos
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
    {
        try
        {
            List<string> Referencia = new List<string> { "EFECTIVO", "TRANSF. BANCARIA", "OTRO" };
            ViewBag.Modo = Referencia;
            RepositorioReparacion repo = new RepositorioReparacion();
            ViewBag.Reparaciones = repo.ObtenerReparaciones();
            var gasto = new Gasto();

            if (id.HasValue && id.Value > 0)
            {
                RepositorioGasto rp = new RepositorioGasto();
                gasto = rp.GetGasto(id.Value) ?? new Gasto();
            }

            return View(gasto);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de pagos");
            TempData["Error"] = "Ocurrió un error al obtener la lista de pagos";
            ViewBag.Error = TempData["Error"];
           return RedirectToAction(nameof(Index));
        }
    }


    //[Authorize]
    public IActionResult Guardar(Gasto gasto)
    {
        try
        {
            RepositorioGasto rp = new RepositorioGasto();

            if (gasto.Id > 0)
            {
                rp.ModificarPago(gasto);
                 TempData["Mensaje"] = "Modificación exitosa.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                rp.AltaGasto(gasto);
                 TempData["Mensaje"] = "Alta exitosa.";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de pagos");
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));

        }
    }
    //[Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id) //Es un anulado logico
    {
        try
        {
            RepositorioGasto rp = new RepositorioGasto();
            RepositorioUsuario ru = new RepositorioUsuario();
            var usuario = ru.ObtenerPorEmail(User.Identity.Name);
            rp.EliminarPago(id);
            TempData["Mensaje"] = "El Gasto ha sido anulado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de pagos");
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));
        }
    }
    //[Authorize]
    public IActionResult Detalles(int id)
    {
        try
        {
            RepositorioGasto rp = new RepositorioGasto();
            RepositorioUsuario ru = new RepositorioUsuario();
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
            ViewBag.UserRole = userRole;
            var p = rp.GetGasto(id);
            return View(p);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de pagos");
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));
        }
    }
    
}
