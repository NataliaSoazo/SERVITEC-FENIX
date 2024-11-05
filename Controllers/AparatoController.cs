using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERVITEC_FENIX.Models;

namespace SERVITEC_FENIX.Controllers
{
    public class AparatoController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AparatoController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // [Authorize]
        public IActionResult Index()
        {
            RepositorioAparato rp = new RepositorioAparato();
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
            ViewBag.UserRole = userRole;
            try
            {
                var lista = rp.ObtenerAparatos();

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
                TempData["Error"] = "No se puede accedes a lista este usuario.";
                throw;
            }

        }
        public IActionResult Editar(int id)
        {
            try
            {
                if (id > 0)
                {
                    RepositorioAparato rp = new RepositorioAparato();
                    var propietario = rp.ObtenerAparato(id);
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
                TempData["Error"] = "Ocurrió un error.";
                throw;
            }
        }
         //[Authorize]
        public IActionResult Guardar(Aparato aparato)
        {
            aparato.NombreA = aparato.NombreA.ToUpper();
            RepositorioAparato rp = new RepositorioAparato();
            try
            {

                if (aparato.Id > 0)
                {
                    rp.ModificarAparato(aparato);
                    TempData["Mensaje"] = "Aparato editado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                    rp.AltaAparato(aparato);
                TempData["Mensaje"] = "Aparato creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error");
                TempData["Error"] = "Ocurrió un error.";
                throw;
            }
        }
        // [Authorize]
        public IActionResult Eliminar(int id)
        {
            RepositorioAparato rp = new RepositorioAparato();
            try
            {
                rp.EliminarAparato(id);
                TempData["Mensaje"] = "Eliminación exitosa";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, "Error");
                TempData["Error"] = "Ocurrió un error.";
                throw;
            }
        }
        // [Authorize]
        public IActionResult Detalles(int id)
        {
            try
            {
                RepositorioAparato rp = new RepositorioAparato();
                var aparato = rp.ObtenerAparato(id);
                return View(aparato);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error");
                TempData["Error"] = "Ocurrió un error.";
                throw;
            }
        }
    }
}
