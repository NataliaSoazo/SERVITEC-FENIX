using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERVITEC_FENIX.Models;

namespace SERVITEC_FENIX.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ClienteController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            RepositorioCliente rp = new RepositorioCliente();

            var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
            ViewBag.UserRole = userRole;
            try
            {
                var lista = rp.ObtenerClientes();

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
                _logger.LogError(ex, "Error al obtener la lista de pagos");
                TempData["Error"] = "Ocurrio un error al obtener la lista de usuarios";
                ViewBag.Error = TempData["Error"];
                throw;
            } 
        }
        public IActionResult Editar(int id)
        {
            try
            {
                if (id > 0)
                {
                    RepositorioCliente rp = new RepositorioCliente();
                    var propietario = rp.ObtenerCliente(id);
                    return View(propietario);
                }
                else
                {
                    return View();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error");
                TempData["Error"] = "Ocurrio un error";
                throw;
            }
        }


        public IActionResult Guardar(Cliente cliente)
        {
            try
            {
                cliente.Nombre = cliente.Nombre.ToUpper();
                cliente.Apellido = cliente.Apellido.ToUpper();
                cliente.Ciudad = cliente.Ciudad.ToUpper();
                cliente.Domicilio = cliente.Domicilio.ToUpper();
                RepositorioCliente rp = new RepositorioCliente();
                if (cliente.Id > 0)
                {
                    rp.ModificarCliente(cliente);
                    TempData["Mensaje"]  = "Datos actualizados correctamente";
                }
                else
                   { rp.AltaCliente(cliente);
                    TempData["Mensaje"] = "Se creó el cliente correctamente ";
                   }
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error");
                TempData["Error"] = "Ocurrio un error al realizar la operación";
                throw;
            }
        }
        
        public IActionResult Eliminar(int id)
        {
            try
            {
                RepositorioCliente rp = new RepositorioCliente();
                rp.EliminarCliente(id);
                TempData["Mensaje"]  = "Se eliminó  el cliente correctamente ";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error  en clientes ");
                TempData["Error"] = "Ocurrio un error";
                 return RedirectToAction(nameof(Index));
                throw;
                
            }
        }
        public IActionResult Detalles(int id)
        {
            try
            {
                RepositorioCliente rp = new RepositorioCliente();
                var propietario = rp.ObtenerCliente(id);
                return View(propietario);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de pagos");
                TempData["Error"] = "No se puede eliminar el cliente";
                throw;

            }
        }
    }

}
