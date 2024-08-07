using MySql.Data.MySqlClient;

namespace SERVITEC_FENIX.Models;

public class RepositorioAparato
{
    readonly string ConnectionString = "Server=localhost;Database=servitec;User=root;Password=;";

    public RepositorioAparato()
    {

    }

    public IList<Aparato> ObtenerAparatos()
    {
        var Aparatos = new List<Aparato>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Aparato.Id)}, {nameof(Aparato.NombreA)} FROM Aparatos";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Aparatos.Add(new Aparato
                        {
                            Id = reader.GetInt32(nameof(Aparato.Id)),
                            NombreA = reader.GetString(nameof(Aparato.NombreA)),
                           
                        });
                     }

                }
            }
         }
         return Aparatos;
    }

    public int AltaAparato(Aparato aparato){
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString)){
            var sql = @$"INSERT INTO aparatos ({nameof(Aparato.NombreA)})
                                            VALUES (@{nameof(Aparato.NombreA)});            
             SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Aparato.NombreA)}", aparato.NombreA);
               

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                aparato.Id = id;
                connection.Close();


            }
        }
        return id;
    }

    public Aparato ObtenerAparato(int id)
    {
        Aparato  aparato= null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Aparato.Id)}, {nameof(Aparato.NombreA)}
            FROM Aparatos WHERE {nameof(Aparato.Id)} = @{nameof(Aparato.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Aparato.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        aparato = new Aparato
                        {
                             Id = reader.GetInt32(nameof(Aparato.Id)),
                            NombreA = reader.GetString(nameof(Aparato.NombreA)),
                           

                        };
                    }
                }
            }
            
        }
        return aparato;
    }

    public int ModificarAparato(Aparato aparato){
        using (var connection = new MySqlConnection(ConnectionString)){
            var sql = @$"UPDATE aparatos 
            SET {nameof(Aparato.NombreA)} = @{nameof(Aparato.NombreA)}
           
            WHERE {nameof(Aparato.Id)} = @{nameof(Aparato.Id)} ";     
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Aparato.Id)}", aparato.Id);
                command.Parameters.AddWithValue($"@{nameof(Aparato.NombreA)}", aparato.NombreA);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

    public int  EliminarAparato(int id){
        using(var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE from aparatos WHERE {nameof(Aparato.Id)} = @{nameof(Aparato.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Aparato.Id)}", id);
                connection.Open();
               command.ExecuteNonQuery();
               connection.Close();
            }
        }
         return 0;
    }

}