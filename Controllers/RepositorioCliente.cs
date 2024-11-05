using MySql.Data.MySqlClient;

namespace SERVITEC_FENIX.Models;

public class RepositorioCliente
{
    readonly string ConnectionString = "Server=localhost;Database=servitec;User=root;Password=;";

    public RepositorioCliente()
    {

    }

    public IList<Cliente> ObtenerClientes()
    {
        var clientes = new List<Cliente>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Cliente.Id)}, {nameof(Cliente.Nombre)}, {nameof(Cliente.Apellido)}, {nameof(Cliente.Domicilio)},
             {nameof(Cliente.Ciudad)}, {nameof(Cliente.Telefono)}, {nameof(Cliente.Correo)}, 
             {nameof(Cliente.Latitud)}, {nameof(Cliente.Longitud)} FROM clientes ORDER BY {nameof(Cliente.Apellido)} DESC";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(new Cliente
                        {
                            Id = reader.GetInt32(nameof(Cliente.Id)),
                            Nombre = reader.GetString(nameof(Cliente.Nombre)),
                            Apellido = reader.GetString(nameof(Cliente.Apellido)),
                            Domicilio = reader.GetString(nameof(Cliente.Domicilio)),
                            Ciudad = reader.GetString(nameof(Cliente.Ciudad)),
                            Correo = reader.GetString(nameof(Cliente.Correo)),
                            Telefono = reader.GetString(nameof(Cliente.Telefono)),
                            Latitud = reader.GetString(nameof(Cliente.Latitud)),
                            Longitud = reader.GetString(nameof(Cliente.Longitud)),

                        });
                     }

                }
            }
         }
         return clientes;
    }

    public int AltaCliente(Cliente cliente){
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString)){
            var sql = @$"INSERT INTO Clientes ({nameof(Cliente.Nombre)}, {nameof(Cliente.Apellido)}, {nameof(Cliente.Domicilio)}, {nameof(Cliente.Ciudad)}, {nameof(Cliente.Telefono)}, {nameof(Cliente.Correo)}, {nameof(Cliente.Latitud)}, {nameof(Cliente.Longitud)})
                                            VALUES (@{nameof(Cliente.Nombre)}, @{nameof(Cliente.Apellido)}, @{nameof(Cliente.Domicilio)}, @{nameof(Cliente.Ciudad)}, @{nameof(Cliente.Telefono)}, @{nameof(Cliente.Correo)}, @{nameof(Cliente.Latitud)}, @{nameof(Cliente.Longitud)});            
             SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Cliente.Nombre)}", cliente.Nombre);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Apellido)}",cliente.Apellido);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Domicilio)}",cliente.Domicilio);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Ciudad)}", cliente.Ciudad);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Correo)}", cliente.Correo);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Telefono)}",cliente.Telefono);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Latitud)}", cliente.Latitud);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Longitud)}", cliente.Longitud);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                cliente.Id = id;
                connection.Close();


            }
        }
        return id;
    }

    public Cliente ObtenerCliente(int id)
    {
        Cliente  cliente= null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Cliente.Id)}, {nameof(Cliente.Nombre)}, {nameof(Cliente.Apellido)}, {nameof(Cliente.Domicilio)}, {nameof(Cliente.Ciudad)}, {nameof(Cliente.Telefono)}, {nameof(Cliente.Correo)}, {nameof(Cliente.Latitud)}, {nameof(Cliente.Longitud)}
            FROM clientes WHERE {nameof(Cliente.Id)} = @{nameof(Cliente.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Cliente.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                             Id = reader.GetInt32(nameof(Cliente.Id)),
                            Nombre = reader.GetString(nameof(Cliente.Nombre)),
                            Apellido = reader.GetString(nameof(Cliente.Apellido)),
                            Domicilio = reader.GetString(nameof(Cliente.Domicilio)),
                            Ciudad = reader.GetString(nameof(Cliente.Ciudad)),
                            Correo = reader.GetString(nameof(Cliente.Correo)),
                            Telefono = reader.GetString(nameof(Cliente.Telefono)),
                            Latitud = reader.GetString(nameof(Cliente.Latitud)),
                            Longitud = reader.GetString(nameof(Cliente.Longitud)),

                        };
                    }
                }
            }
            
        }
        return cliente;
    }

    public int ModificarCliente(Cliente cliente){
        using (var connection = new MySqlConnection(ConnectionString)){
            var sql = @$"UPDATE clientes 
            SET {nameof(Cliente.Nombre)} = @{nameof(Cliente.Nombre)},
            {nameof(Cliente.Apellido)} = @{nameof(Cliente.Apellido)},
            {nameof(Cliente.Domicilio)} = @{nameof(Cliente.Domicilio)},
            {nameof(Cliente.Ciudad)} = @{nameof(Cliente.Ciudad)},
            {nameof(Cliente.Telefono)} = @{nameof(Cliente.Telefono)},
            {nameof(Cliente.Correo)} = @{nameof(Cliente.Correo)},
            {nameof(Cliente.Latitud)} = @{nameof(Cliente.Latitud)},
            {nameof(Cliente.Longitud)} = @{nameof(Cliente.Longitud)}
            WHERE {nameof(Cliente.Id)} = @{nameof(Cliente.Id)} ";     
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Cliente.Id)}", cliente.Id);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Nombre)}", cliente.Nombre);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Apellido)}", cliente.Apellido);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Domicilio)}", cliente.Domicilio);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Ciudad)}", cliente.Ciudad);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Telefono)}", cliente.Telefono);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Correo)}", cliente.Correo);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Latitud)}", cliente.Latitud);
                command.Parameters.AddWithValue($"@{nameof(Cliente.Longitud)}", cliente.Longitud);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

    public int  EliminarCliente(int id){
        using(var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE from clientes WHERE {nameof(Cliente.Id)} = @{nameof(Cliente.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Cliente.Id)}", id);
                connection.Open();
               command.ExecuteNonQuery();
               connection.Close();
            }
        }
         return 0;
    }

}