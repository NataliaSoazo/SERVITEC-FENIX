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

          RepositorioCliente repoCliente = new RepositorioCliente();
            ViewBag.Clientes = repoCliente.ObtenerClientes();
            RepositorioAparato repoAparato = new RepositorioAparato();
            ViewBag.Aparatos = repoAparato.ObtenerAparatos();
            RepositorioMarca repoMarca = new RepositorioMarca();
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
                //var currentDateTime = DateTime.Now.ToString("yyyy-MM-dd");
                //ViewBag.CurrentDateTime = currentDateTime;
                return View();
            }
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = "No se pudo  realizar la acción. ";
            return View();
        }
    }




    public ActionResult Guardar(OrdenReparacion ordenReparacion)
    {   
        ordenReparacion.Falla = ordenReparacion.Falla.ToUpper();
        ordenReparacion.NroSerie = ordenReparacion.NroSerie.ToUpper();
        Boolean validado = rr.validarOrdenReparacion(ordenReparacion);
       
        
            if (validado == true)
            {

                if (ordenReparacion.Id > 0)
                {
                    rr.ModificarOrdenReparacion(ordenReparacion);
                    return RedirectToAction(nameof(Index));

                }
                else 
                    ordenReparacion.CodigoReparacion = ordenReparacion.FechaRecepcion.Year + "-"+ ordenReparacion.FechaRecepcion.Month + "-";
                    rr.AltaOrdenReparacion(ordenReparacion);
                    return RedirectToAction(nameof(Index));
                 
            }else  ViewBag.Error = "La orden debe pertenecer al año en curso";
                return View("Error");
    

    }



public IActionResult Eliminar(int id)
    {   try{
        rr.EliminarOrdenReparacion(id);
        return RedirectToAction(nameof(Index));
    }  catch (Exception ex)
            {//poner breakpoints para detectar errores
                throw;
            }

    }

    public IActionResult Detalles( int id)
    {   try{
            var c = rr.ObtenerOrdenReparacion(id);
            return View(c); 
    }  catch (Exception ex)
            {//poner breakpoints para detectar errores
                throw;
            }
    }   
}
