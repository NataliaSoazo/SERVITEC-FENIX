using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SERVITEC_FENIX.Models;
using ZstdSharp.Unsafe;
using System;
using System.Runtime.ConstrainedExecution;
using System.Linq.Expressions;

namespace SERVITEC_FENIX.Controllers;

public class OrdenReparacionController : Controller
{
    RepositorioOrdenReparacion rr = new RepositorioOrdenReparacion();
    private readonly ILogger<HomeController> _logger;

    public OrdenReparacionController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()

    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        RepositorioCliente repoCliente = new RepositorioCliente();
        RepositorioAparato repoAparato = new RepositorioAparato();
        RepositorioMarca repoMarca = new RepositorioMarca();
        try
        {

            ViewBag.Clientes = repoCliente.ObtenerClientes();
            ViewBag.Aparatos = repoAparato.ObtenerAparatos();
            ViewBag.Marcas = repoMarca.ObtenerMarcas();
            var lista = rr.ObtenerOrdenReparaciones();
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
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error");
            TempData["Error"] = "Ocurrio un error ";
            throw;
        }

    }

    public IActionResult Editar(int id)
    {
        try
        {

            RepositorioCliente repoCliente = new RepositorioCliente();
            ViewBag.Clientes = repoCliente.ObtenerClientes();
            RepositorioAparato repoAparato = new RepositorioAparato();
            ViewBag.Aparatos = repoAparato.ObtenerAparatos();
            RepositorioMarca repoMarca = new RepositorioMarca();
            ViewBag.Marcas = repoMarca.ObtenerMarcas();

            if (id > 0)
            {
                var ordenReparacion = rr.ObtenerOrdenReparacion(id);
                return View(ordenReparacion);
            }
            else
            {
                return View();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error");
            TempData["Error"] = "No se pudo  realizar la acción. ";
            return View();
        }
    }




    public ActionResult Guardar(OrdenReparacion ordenReparacion)
    {
        ordenReparacion.Falla = ordenReparacion.Falla.ToUpper();
        ordenReparacion.NroSerie = ordenReparacion.NroSerie.ToUpper();
        Boolean validado = rr.validarOrdenReparacion(ordenReparacion);
        try
        {
            if (validado == true)
            {

                if (ordenReparacion.Id > 0)
                {
                    rr.ModificarOrdenReparacion(ordenReparacion);
                    TempData["Mensaje"] = "Modificación exitosa.";
                    return RedirectToAction(nameof(Index));

                }
                else{
                    ordenReparacion.CodigoReparacion = ordenReparacion.FechaRecepcion.Year + "-" + ordenReparacion.FechaRecepcion.Month + "-";
               
                rr.AltaOrdenReparacion(ordenReparacion);
                TempData["Mensaje"] = "Alta exitosa.";
                return RedirectToAction(nameof(Index));
                }

            }
            else ViewBag.Error = "La orden debe pertenecer al año en curso";
            return View("Error");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error");
            TempData["Error"] = "No se pudo  realizar la acción. ";
            return View();
        }


    }



    public IActionResult Eliminar(int id)
    {
        try
        {
            rr.EliminarOrdenReparacion(id);
            TempData["Mensaje"] = "Eliminación Exitosa. ";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {//poner breakpoints para detectar errores
            _logger.LogError(ex, "Error");
            TempData["Error"] = "No se pudo  realizar la acción. ";
            return View();
        }

    }

    public IActionResult Detalles(int id)
    {
        try
        {
            var c = rr.ObtenerOrdenReparacion(id);
            return View(c);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error");
            TempData["Error"] = "No se pudo  realizar la acción. ";
            return View();
        }
    }
}
