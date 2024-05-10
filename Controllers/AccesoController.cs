using Microsoft.AspNetCore.Mvc;
using SERVITEC_FENIX.Models;
namespace SERVITEC_FENIX.Controllers
{
    public class AccesoController : Controller
    {   [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Usuario usuario)
        {   
        RepositorioUsuario repo = new RepositorioUsuario();
            // Llama al método Ingresar para verificar las credenciales
        var user = repo.getUsuario(usuario.Correo);

        if (user.Correo == usuario.Correo && user.Clave == usuario.Clave)
        {
                // Usuario válido, puedes devolver los detalles del usuario o un token de autenticación
                //  return Ok(usuario);
                return RedirectToAction("Index", "Home");
        }
        else
        {
            // Credenciales inválidas, devolver un error o un mensaje apropiado
            return Unauthorized("Credenciales inválidas");
        }
    }
 }
}
