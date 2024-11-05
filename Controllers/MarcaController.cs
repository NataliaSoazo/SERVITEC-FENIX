using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERVITEC_FENIX.Models;

namespace SERVITEC_FENIX.Controllers
{
    public class MarcaController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public MarcaController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
            ViewBag.UserRole = userRole;
            RepositorioMarca rp = new RepositorioMarca();
            try
            {
                var lista = rp.ObtenerMarcas();

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
                TempData["Error"] = "Ocurrio un error al obtener el listado";
                throw;
            }
        }
        public IActionResult Editar(int id)
        {
            try
            {
                if (id > 0)
                {
                    RepositorioMarca rp = new RepositorioMarca();
                    var propietario = rp.ObtenerMarca(id);
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

        public IActionResult Guardar(Marca marca)
        {
            try
            {
                marca.NombreM = marca.NombreM.ToUpper();
                RepositorioMarca rp = new RepositorioMarca();
                if (marca.Id > 0)
                {
                    rp.ModificarMarca(marca);
                    TempData["Mensaje"] = "Modificación exitosa";
                }
                else
                    rp.AltaMarca(marca);
                TempData["Mensaje"] = "Alta exitosa";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de pagos");
                TempData["Error"] = "Ocurrio un error al obtener la lista de usuarios";
                ViewBag.Error = TempData["Error"];
                throw;
            }
        }
        public IActionResult Eliminar(int id)
        {
            try
            {
                RepositorioMarca rp = new RepositorioMarca();
                rp.EliminarMarca(id);
                TempData["Mensaje"] = "Eliminación exitosa";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de pagos");
                TempData["Error"] = "Ocurrio un error al obtener la lista de usuarios";
                ViewBag.Error = TempData["Error"];
                throw;
            }
        }
        public IActionResult Detalles(int id)
        {
            try
            {
                RepositorioMarca rp = new RepositorioMarca();
                var propietario = rp.ObtenerMarca(id);
                return View(propietario);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de pagos");
                TempData["Error"] = "Ocurrio un error al obtener la lista de usuarios";
                ViewBag.Error = TempData["Error"];
                throw;
            }
        }
    }

}
