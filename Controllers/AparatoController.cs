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

    public IActionResult Index()
    {
        RepositorioAparato rp = new RepositorioAparato();
        var lista = rp.ObtenerAparatos();
        return View(lista);
    }
    public IActionResult Editar(int id)
    {
        if (id > 0){
            RepositorioAparato rp = new RepositorioAparato();
            var propietario = rp.ObtenerAparato(id);
            return View(propietario); 
        } else {
            return View();
        }
    }
    
    public IActionResult Guardar( Aparato aparato)
    {  
        aparato.NombreA = aparato.NombreA.ToUpper();
        RepositorioAparato rp = new RepositorioAparato();
        if(aparato.Id > 0){
            rp.ModificarAparato( aparato);

        }else
        rp.AltaAparato(aparato);
        return RedirectToAction(nameof(Index));
    }
     public IActionResult Eliminar( int id)
    {  
        RepositorioAparato rp = new RepositorioAparato();
        rp.EliminarAparato(id);
        return RedirectToAction(nameof(Index));
    }    
    public IActionResult Detalles( int id)
    {  
        RepositorioAparato rp = new RepositorioAparato();
            var aparato = rp.ObtenerAparato(id);
            return View(aparato); 
    }    
}

}
