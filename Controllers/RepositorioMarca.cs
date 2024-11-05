using MySql.Data.MySqlClient;

namespace SERVITEC_FENIX.Models;

public class RepositorioMarca
{
    readonly string ConnectionString = "Server=localhost;Database=servitec;User=root;Password=;";

    public RepositorioMarca()
    {

    }

    public IList<Marca> ObtenerMarcas()
    {
        var marcas = new List<Marca>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Marca.Id)}, {nameof(Marca.NombreM)} FROM Marcas ORDER BY {nameof(Marca.NombreM)} DESC";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        marcas.Add(new Marca
                        {
                            Id = reader.GetInt32(nameof(Marca.Id)),
                            NombreM = reader.GetString(nameof(Marca.NombreM)),
                           
                        });
                     }

                }
            }
         }
         return marcas;
    }

    public int AltaMarca(Marca marca){
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString)){
            var sql = @$"INSERT INTO Marcas ({nameof(Marca.NombreM)})
                                            VALUES (@{nameof(Marca.NombreM)});            
             SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Marca.NombreM)}", marca.NombreM);
               

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                marca.Id = id;
                connection.Close();


            }
        }
        return id;
    }

    public Marca ObtenerMarca(int id)
    {
        Marca  marca= null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Marca.Id)}, {nameof(Marca.NombreM)}
            FROM marcas WHERE {nameof(Marca.Id)} = @{nameof(Marca.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Marca.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        marca = new Marca
                        {
                             Id = reader.GetInt32(nameof(Marca.Id)),
                            NombreM = reader.GetString(nameof(Marca.NombreM)),
                           

                        };
                    }
                }
            }
            
        }
        return marca;
    }

    public int ModificarMarca(Marca marca){
        using (var connection = new MySqlConnection(ConnectionString)){
            var sql = @$"UPDATE marcas 
            SET {nameof(Marca.NombreM)} = @{nameof(Marca.NombreM)}
           
            WHERE {nameof(Marca.Id)} = @{nameof(Marca.Id)} ";     
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Marca.Id)}", marca.Id);
                command.Parameters.AddWithValue($"@{nameof(Marca.NombreM)}", marca.NombreM);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

    public int  EliminarMarca(int id){
        using(var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE from marcas WHERE {nameof(Marca.Id)} = @{nameof(Marca.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Marca.Id)}", id);
                connection.Open();
               command.ExecuteNonQuery();
               connection.Close();
            }
        }
         return 0;
    }

}