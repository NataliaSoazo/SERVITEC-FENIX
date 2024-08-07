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
        var lista = rp.ObtenerClientes();
        return View(lista);
    }
    public IActionResult Editar(int id)
    {
        if (id > 0){
            RepositorioCliente rp = new RepositorioCliente();
            var propietario = rp.ObtenerCliente(id);
            return View(propietario); 
        } else {
            return View();
        }
    }
    
    
    public IActionResult Guardar( Cliente cliente)
    {  
        cliente.Nombre = cliente.Nombre.ToUpper();
        cliente.Apellido = cliente.Apellido.ToUpper();
        cliente.Ciudad = cliente.Ciudad.ToUpper();
        cliente.Domicilio = cliente.Domicilio.ToUpper();
        
        
        RepositorioCliente rp = new RepositorioCliente();
        if(cliente.Id > 0){
            rp.ModificarCliente(cliente);

        }else
        rp.AltaCliente(cliente);
        return RedirectToAction(nameof(Index));
    }
     public IActionResult Eliminar( int id)
    {  
        RepositorioCliente rp = new RepositorioCliente();
        rp.EliminarCliente(id);
        return RedirectToAction(nameof(Index));
    }    
    public IActionResult Detalles( int id)
    {  
        RepositorioCliente rp = new RepositorioCliente();
            var propietario = rp.ObtenerCliente(id);
            return View(propietario); 
    }    
}

}
