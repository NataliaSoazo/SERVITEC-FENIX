using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

namespace SERVITEC_FENIX.Models;

public class RepositorioOrdenReparacion
{
    readonly string ConnectionString = "Server=localhost;Database=servitec;User=root;Password=;";

    public RepositorioOrdenReparacion()
    {

    }

    public IList<OrdenReparacion> ObtenerOrdenReparaciones()
    {
        var ordenesReparacion = new List<OrdenReparacion>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT i.{nameof(OrdenReparacion.Id)}, {nameof(OrdenReparacion.CodigoReparacion)}, {nameof(OrdenReparacion.FechaRecepcion)}, {nameof(OrdenReparacion.IdCliente)}, c.{nameof(OrdenReparacion.DatosCliente.Nombre)}, c.{nameof(OrdenReparacion.DatosCliente.Apellido)}, {nameof(OrdenReparacion.IdAparato)}, {nameof(OrdenReparacion.IdMarca)},
            {nameof(OrdenReparacion.Falla)},{nameof(OrdenReparacion.NroSerie)}, {nameof(OrdenReparacion.Valor)}
                FROM Ordenesreparacion i 
                INNER JOIN clientes c ON i.{nameof(OrdenReparacion.IdCliente)} = c.{nameof(Cliente.Id)}
                INNER JOIN aparatos a ON i.{nameof(OrdenReparacion.IdAparato)} = a.{nameof(Aparato.Id)}
                INNER JOIN marcas m ON i.{nameof(OrdenReparacion.IdMarca)} = m.{nameof(Marca.Id)}
                ";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ordenesReparacion.Add(new OrdenReparacion
                        {
                           Id = reader.GetInt32(nameof(OrdenReparacion.Id)),
                            CodigoReparacion = reader.GetString(nameof(OrdenReparacion.CodigoReparacion)),
							FechaRecepcion = reader.GetDateTime(nameof(OrdenReparacion.FechaRecepcion)),
							IdCliente = reader.GetInt32(nameof(OrdenReparacion.IdCliente)),
                            DatosCliente = new Cliente
                            {
                                Nombre = reader.GetString(nameof(Cliente.Nombre)),
                                Apellido = reader.GetString (nameof(Cliente.Apellido)),
                            },
                            IdAparato = reader.GetInt32(nameof(OrdenReparacion.IdAparato)),
							Falla= reader.GetString(nameof(OrdenReparacion.Falla)),
                            NroSerie= reader.GetString(nameof(OrdenReparacion.NroSerie)),
                            Valor = reader.GetDouble(nameof(OrdenReparacion.Valor)),
						
                            
                        });
                     }

                }
            }
         }
         return ordenesReparacion;
    }


  
    public int AltaOrdenReparacion(OrdenReparacion ordenReparacion){
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString)){
            string sql = $@"INSERT INTO ordenesreparacion 
            ({nameof(OrdenReparacion.CodigoReparacion)}, 
            {nameof(OrdenReparacion.FechaRecepcion)}, 
            {nameof(OrdenReparacion.IdCliente)}, 
            {nameof(OrdenReparacion.IdAparato)}, 
            {nameof(OrdenReparacion.IdMarca)},
            {nameof(OrdenReparacion.Falla)},
            {nameof(OrdenReparacion.NroSerie)}, 
            {nameof(OrdenReparacion.Valor)})
                VALUES (@{nameof(OrdenReparacion.CodigoReparacion)}, 
                @{nameof(OrdenReparacion.FechaRecepcion)}, 
                @{nameof(OrdenReparacion.IdCliente)}, 
                @{nameof(OrdenReparacion.IdAparato)},
                @{nameof(OrdenReparacion.IdMarca)},
                @{nameof(OrdenReparacion.Falla)}, 
                @{nameof(OrdenReparacion.NroSerie)}, 
                @{nameof(OrdenReparacion.Valor)});           
                SELECT LAST_INSERT_ID();";
           
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.CodigoReparacion)}", ordenReparacion.CodigoReparacion);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.FechaRecepcion)}", ordenReparacion.FechaRecepcion);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.IdCliente)}", ordenReparacion.IdCliente);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.IdAparato)}", ordenReparacion.IdAparato);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.IdMarca)}", ordenReparacion.IdMarca);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.Falla)}", ordenReparacion.Falla);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.NroSerie)}", ordenReparacion.NroSerie);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.Valor)}", ordenReparacion.Valor);
               
                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                ordenReparacion.Id = id;
                //command.ExecuteNonQuery();
                connection.Close();

            }
             
        }
         string Reparacion = ordenReparacion.CodigoReparacion + ordenReparacion.Id ;
             using (var connection2 = new MySqlConnection(ConnectionString)){
             string sql2 = $@"UPDATE ordenesReparacion SET 
                    {nameof(OrdenReparacion.CodigoReparacion)} = @{nameof(OrdenReparacion.CodigoReparacion)}
                WHERE {nameof(OrdenReparacion.Id)} = LAST_INSERT_ID() ";           
            using (var command2 = new MySqlCommand(sql2, connection2)){
                
                command2.Parameters.AddWithValue($"@{nameof(OrdenReparacion.CodigoReparacion)}", Reparacion);
                connection2.Open();
                command2.ExecuteNonQuery();
                connection2.Close();

            }
            }    
       
        return id;
        

    }
    
    public OrdenReparacion ObtenerOrdenReparacion(int id)
    {
        OrdenReparacion ordenReparacion = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {    string sql = $@"SELECT i.{nameof(OrdenReparacion.Id)}, {nameof(OrdenReparacion.CodigoReparacion)},
            {nameof(OrdenReparacion.FechaRecepcion)}, {nameof(OrdenReparacion.IdCliente)}, 
            c.{nameof(Cliente.Nombre)}, c.{nameof(Cliente.Apellido)}, 
            {nameof(OrdenReparacion.IdAparato)}, a.{nameof(Aparato.NombreA)},
            {nameof(OrdenReparacion.IdMarca)}, m.{nameof(Marca.NombreM)},
            {nameof(OrdenReparacion.Falla)},{nameof(OrdenReparacion.NroSerie)}, 
            {nameof(OrdenReparacion.Valor)}
               
                FROM ordenesreparacion i INNER JOIN clientes c ON i.{nameof(OrdenReparacion.IdCliente)} = c.{nameof(Cliente.Id)}
                                        INNER JOIN aparatos a ON i.{nameof(OrdenReparacion.IdAparato)} = a.{nameof(Aparato.Id)}
                                        INNER JOIN marcas m ON i.{nameof(OrdenReparacion.IdMarca)} = m.{nameof(Marca.Id)}
                WHERE i.{nameof(OrdenReparacion.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ordenReparacion = new OrdenReparacion
                        {
                            Id = reader.GetInt32(nameof(OrdenReparacion.Id)),
                            CodigoReparacion = reader.GetString(nameof(OrdenReparacion.CodigoReparacion)),
							FechaRecepcion = reader.GetDateTime(nameof(OrdenReparacion.FechaRecepcion)),
							IdCliente = reader.GetInt32(nameof(OrdenReparacion.IdCliente)),
                            DatosCliente = new Cliente
                            {
                                Nombre = reader.GetString(nameof(Cliente.Nombre)),
                                Apellido = reader.GetString (nameof(Cliente.Apellido)),
                            },
                            IdAparato = reader.GetInt32(nameof(OrdenReparacion.IdAparato)),
                            DatosAparato = new Aparato
                            {
                                NombreA = reader.GetString(nameof(Aparato.NombreA))
                            },
                            
							IdMarca = reader.GetInt32(nameof(OrdenReparacion.IdMarca)),
                            DatosMarca = new Marca
                            {
                                NombreM = reader.GetString(nameof(Marca.NombreM))
                            },
							Falla= reader.GetString(nameof(OrdenReparacion.Falla)),
                            NroSerie= reader.GetString(nameof(OrdenReparacion.NroSerie)),
                            Valor = reader.GetDouble(nameof(OrdenReparacion.Valor)),
                        };
                    }
                }
            }
            
        }
        return ordenReparacion;
    }

    public int ModificarOrdenReparacion(OrdenReparacion ordenReparacion){
        using (var connection = new MySqlConnection(ConnectionString)){
            string sql = $@"UPDATE ordenesReparacion SET 
                    {nameof(OrdenReparacion.FechaRecepcion)} = @{nameof(OrdenReparacion.FechaRecepcion)},
                    {nameof(OrdenReparacion.IdCliente)} = @{nameof(OrdenReparacion.IdCliente)},
                    {nameof(OrdenReparacion.IdAparato)} = @{nameof(OrdenReparacion.IdAparato)},
                    {nameof(OrdenReparacion.IdMarca)} = @{nameof(OrdenReparacion.IdMarca)},
                    {nameof(OrdenReparacion.Falla)} = @{nameof(OrdenReparacion.Falla)},
                    {nameof(OrdenReparacion.NroSerie)} = @{nameof(OrdenReparacion.NroSerie)},
                    {nameof(OrdenReparacion.Valor)} = @{nameof(OrdenReparacion.Valor)}
                WHERE {nameof(OrdenReparacion.Id)} = @{nameof(OrdenReparacion.Id)}";           
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.Id)}", ordenReparacion.Id);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.FechaRecepcion)}", ordenReparacion.FechaRecepcion);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.IdCliente)}", ordenReparacion.IdCliente);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.IdAparato)}", ordenReparacion.IdAparato);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.IdMarca)}", ordenReparacion.IdMarca);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.Falla)}", ordenReparacion.Falla);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.NroSerie)}", ordenReparacion.NroSerie);
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.Valor)}", ordenReparacion.Valor);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
        }
       
        return 0;

    }
    

    public int  EliminarOrdenReparacion(int id){
        using(var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE from ordenesReparacion WHERE {nameof(OrdenReparacion.Id)} = @{nameof(OrdenReparacion.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(OrdenReparacion.Id)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
         return 0;
    }
    public Boolean validarOrdenReparacion(OrdenReparacion ordenReparacion){
         var fechaActual = DateTime.Now;
       
        if(ordenReparacion.FechaRecepcion<=fechaActual)  {
            return true;
        }else
        return false;
    }

}