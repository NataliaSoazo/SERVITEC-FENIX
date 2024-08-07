using MySql.Data.MySqlClient;
using SERVITEC_FENIX.Controllers;
using ZstdSharp.Unsafe;

namespace SERVITEC_FENIX.Models;

public class RepositorioReparacion
{
    readonly string ConnectionString = "Server=localhost;Database=servitec;User=root;Password=;";

    public RepositorioReparacion()
    {

    }

    public IList<Reparacion> ObtenerReparaciones()
    {
        var reparacion = new List<Reparacion>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT i.{nameof(Reparacion.Id)}, i.{nameof(Reparacion.IdOrden)}, c.{nameof(Cliente.Nombre)}, c.{nameof(Cliente.Apellido)}, {nameof(Reparacion.FechaReparacion)}, {nameof(Reparacion.Detalle)},
            {nameof(Reparacion.FechaEntrega)},{nameof(Reparacion.Pago)}
                FROM reparaciones i 
                INNER JOIN ordenesreparacion o ON i.{nameof(Reparacion.IdOrden)} = o.{nameof(OrdenReparacion.Id)}
                INNER JOIN clientes c ON o.{nameof(OrdenReparacion.IdCliente)} = c.{nameof(Cliente.Id)}
                ";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reparacion.Add(new Reparacion
                        {
                            Id = reader.GetInt32(nameof(Reparacion.Id)),
                            IdOrden = reader.GetInt32(nameof(Reparacion.IdOrden)),
							
                            DatosOrden = new OrdenReparacion
                            {   
                                DatosCliente = new Cliente
                                {
                                    Nombre = reader.GetString(nameof(Cliente.Nombre)),
                                    Apellido = reader.GetString (nameof(Cliente.Apellido)),
                                }
                            },
                            FechaReparacion = reader.GetDateTime(nameof(Reparacion.FechaReparacion)),
						    Detalle = reader.GetString(nameof(Reparacion.Detalle)),
                            FechaEntrega = reader.GetDateTime(nameof(Reparacion.FechaEntrega)),
							Pago= reader.GetDouble(nameof(Reparacion.Pago)),
                          
                            
                        });
                     }

                }
            }
         }
         return reparacion;
    }


  
    public int AltaReparacion(Reparacion reparacion){
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString)){
            string sql = $@"INSERT INTO reparaciones ({nameof(Reparacion.IdOrden)},  {nameof(Reparacion.FechaReparacion)},
            {nameof(Reparacion.Detalle)},{nameof(Reparacion.FechaEntrega)}, {nameof(Reparacion.Pago)})
                VALUES  (@{nameof(Reparacion.IdOrden)},  @{nameof(Reparacion.FechaReparacion)},
            @{nameof(Reparacion.Detalle)}, @{nameof(Reparacion.FechaEntrega)}, @{nameof(Reparacion.Pago)});           
                SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Reparacion.IdOrden)}", reparacion.IdOrden);
                command.Parameters.AddWithValue($"@{nameof(Reparacion.FechaReparacion)}", reparacion.FechaReparacion);
                command.Parameters.AddWithValue($"@{nameof(Reparacion.Detalle)}", reparacion.Detalle);
                command.Parameters.AddWithValue($"@{nameof(Reparacion.FechaEntrega)}", reparacion.FechaEntrega);
                command.Parameters.AddWithValue($"@{nameof(Reparacion.Pago)}", reparacion.Pago);
                
                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                reparacion.Id = id;
                connection.Close();
            }
        }
        return id;
    }

    public Reparacion ObtenerReparacion(int id)
    {
        Reparacion reparacion = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {      string sql = $@"SELECT i.{nameof(Reparacion.Id)}, i.{nameof(Reparacion.IdOrden)}, c.{nameof(Cliente.Nombre)}, c.{nameof(Cliente.Apellido)}, {nameof(Reparacion.FechaReparacion)}, {nameof(Reparacion.Detalle)},
            {nameof(Reparacion.FechaEntrega)},{nameof(Reparacion.Pago)}
                FROM reparaciones i
                INNER JOIN ordenesreparacion o ON i.{nameof(Reparacion.IdOrden)} = o.{nameof(OrdenReparacion.Id)}
                INNER JOIN clientes c ON o.{nameof(OrdenReparacion.IdCliente)} = c.{nameof(Cliente.Id)}
                WHERE i.{nameof(Reparacion.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Reparacion.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reparacion = new Reparacion
                        {
                            Id = reader.GetInt32(nameof(Reparacion.Id)),
                            IdOrden = reader.GetInt32(nameof(Reparacion.IdOrden)),
							
                            DatosOrden = new OrdenReparacion
                            {   
                                DatosCliente = new Cliente
                                {
                                    Nombre = reader.GetString(nameof(Cliente.Nombre)),
                                    Apellido = reader.GetString (nameof(Cliente.Apellido)),
                                }
                            },
                            FechaReparacion = reader.GetDateTime(nameof(Reparacion.FechaReparacion)),
						    Detalle = reader.GetString(nameof(Reparacion.Detalle)),
                            FechaEntrega = reader.GetDateTime(nameof(Reparacion.FechaEntrega)),
							Pago= reader.GetDouble(nameof(Reparacion.Pago)),
                          
                            
                        };
                    }
                }
            }
            
        }
        return reparacion;
    }

    public int ModificarReparacion(Reparacion reparacion){
        using (var connection = new MySqlConnection(ConnectionString)){
            string sql = $@"UPDATE reparaciones SET 
                    {nameof(Reparacion.IdOrden)} = @{nameof(Reparacion.IdOrden)},
                    {nameof(Reparacion.FechaReparacion)} = @{nameof(Reparacion.FechaReparacion)},
                    {nameof(Reparacion.Detalle)} = @{nameof(Reparacion.Detalle)},
                    {nameof(Reparacion.FechaEntrega)} = @{nameof(Reparacion.FechaEntrega)},
                    {nameof(Reparacion.Pago)} = @{nameof(Reparacion.Pago)}
                WHERE {nameof(Reparacion.Id)} = @{nameof(Reparacion.Id)}";           
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Reparacion.Id)}", reparacion.Id);
                command.Parameters.AddWithValue($"@{nameof(Reparacion.IdOrden)}", reparacion.IdOrden);
                command.Parameters.AddWithValue($"@{nameof(Reparacion.FechaReparacion)}", reparacion.FechaReparacion);
                command.Parameters.AddWithValue($"@{nameof(Reparacion.Detalle)}", reparacion.Detalle);
                command.Parameters.AddWithValue($"@{nameof(Reparacion.FechaEntrega)}", reparacion.FechaEntrega);
                command.Parameters.AddWithValue($"@{nameof(Reparacion.Pago)}", reparacion.Pago);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
        }
        return 0;
    }

    public int  EliminarReparacion(int id){
        using(var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE from Reparaciones WHERE {nameof(Reparacion.Id)} = @{nameof(Reparacion.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Reparacion.Id)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
         return 0;
    }
    public Boolean validarReparacion(Reparacion Reparacion){
         var fechaActual = DateTime.Now;
       
        if(Reparacion.FechaReparacion<=fechaActual)  {
            return true;
        }else
        return false;
    }

    

}