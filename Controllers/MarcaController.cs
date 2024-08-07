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
        RepositorioMarca rp = new RepositorioMarca();
        var lista = rp.ObtenerMarcas();
        return View(lista);
    }
    public IActionResult Editar(int id)
    {
        if (id > 0){
            RepositorioMarca rp = new RepositorioMarca();
            var propietario = rp.ObtenerMarca(id);
            return View(propietario); 
        } else {
            return View();
        }
    }
    
    public IActionResult Guardar( Marca marca)
    {  
        marca.NombreM = marca.NombreM.ToUpper();
        RepositorioMarca rp = new RepositorioMarca();
        if(marca.Id > 0){
            rp.ModificarMarca( marca);

        }else
        rp.AltaMarca(marca);
        return RedirectToAction(nameof(Index));
    }
     public IActionResult Eliminar( int id)
    {  
        RepositorioMarca rp = new RepositorioMarca();
        rp.EliminarMarca(id);
        return RedirectToAction(nameof(Index));
    }    
    public IActionResult Detalles( int id)
    {  
        RepositorioMarca rp = new RepositorioMarca();
            var propietario = rp.ObtenerMarca(id);
            return View(propietario); 
    }    
}

}
