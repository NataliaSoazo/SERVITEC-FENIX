using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SERVITEC_FENIX.Models;
using ZstdSharp.Unsafe;
using System;
using System.Runtime.ConstrainedExecution;
using System.Linq.Expressions;

namespace SERVITEC_FENIX.Controllers;

public class ReparacionController : Controller
{
    RepositorioReparacion rr = new RepositorioReparacion();
     private readonly ILogger<HomeController> _logger;

    public ReparacionController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()

    {
        RepositorioReparacion  rr =  new RepositorioReparacion();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        List<string> Referencia = new List<string>{ "EFECTIVO","TRANSFERENCIA BANCARIA", "OTRO"};
        ViewBag.Modo = Referencia;
         var lista = rr.ObtenerReparaciones();

        return View(lista);
    }

    public IActionResult Editar(int id)
    {
        try
        {   
   
            RepositorioOrdenReparacion repoOrden = new RepositorioOrdenReparacion();
            ViewBag.Ordenes = repoOrden.ObtenerOrdenReparaciones();
            
            if (id > 0)
            {
                var Reparacion = rr.ObtenerReparacion(id);
                
                if (TempData.ContainsKey("Mensaje"))
                    {
                        ViewBag.Mensaje = TempData["Mensaje"];
                    }
                    else if (TempData.ContainsKey("Error"))
                    {
                        ViewBag.Error = TempData["Error"];
                    }
                 return View(Reparacion);
             }
         
            else
            {   
                return View();
            }
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Error");
            TempData["Mensaje"] = "No se pudo  realizar la acción. ";
            return View();
        }
    }




    public ActionResult Guardar(Reparacion reparacion)
    {
        try
        {

            if (reparacion.Id > 0)
            {
                rr.ModificarReparacion(reparacion);
                 TempData["Mensaje"] = "Modificacion Exitosa. ";
                return RedirectToAction(nameof(Index));

            }
            else
                rr.AltaReparacion(reparacion);
                 TempData["Mensaje"] = "Alta Exitosa. ";
                return RedirectToAction(nameof(Index));

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error");
            throw;
        }
    }



    public IActionResult Eliminar(int id)
    {   try{
        rr.EliminarReparacion(id);
         TempData["Mensaje"] = "Eliminación Exitosa. ";
        return RedirectToAction(nameof(Index));
    }  catch (Exception ex)
            {
                 _logger.LogError(ex, "Error");
                throw;
            }

    }

    public IActionResult Detalles( int id)
    {   try{
            var c = rr.ObtenerReparacion(id);
            return View(c); 
    }  catch (Exception ex)
            {
                 _logger.LogError(ex, "Error");
                throw;
            }
    }   
}
