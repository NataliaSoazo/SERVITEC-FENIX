using MySql.Data.MySqlClient;

namespace SERVITEC_FENIX.Models;

public class RepositorioGasto
{
    readonly string ConnectionString = "Server=localhost;Database=servitec;User=root;Password=;";

    public RepositorioGasto()
    {

    }

    public IList<Gasto> GetGastos()
    {
        var gastos = new List<Gasto>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Gasto.Id)},{nameof(Gasto.NombreGasto)}, {nameof(Gasto.Fecha)}, {nameof(Gasto.Modo)}, {nameof(Gasto.Importe)}, {nameof(Gasto.Anulado)}
               FROM gastos p 
                WHERE p.{nameof(Gasto.Anulado)} = 'NO' 
                ORDER BY p.{nameof(Gasto.Fecha)} DESC";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        gastos.Add(new Gasto
                        {
                            Id = reader.GetInt32(nameof(Gasto.Id)),
                            NombreGasto = reader.GetString(nameof(Gasto.NombreGasto)),
                            Fecha = reader.GetDateTime(nameof(Gasto.Fecha)),
                            Modo = reader.GetString(nameof(Gasto.Modo)),
                            Importe = reader.GetDouble(nameof(Gasto.Importe)),
                            Anulado = reader.GetString(nameof(Gasto.Anulado))
                        });
                    }

                }
            }
        }
        return gastos;
    }


    public int AltaGasto(Gasto gasto)
    {
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"INSERT INTO gastos ({nameof(Gasto.Fecha)}, {nameof(Gasto.Modo)}, {nameof(Gasto.Importe)}, {nameof(Gasto.Anulado)}, {nameof(Gasto.NombreGasto)}) 
                        VALUES (@{nameof(Gasto.Fecha)}, @{nameof(Gasto.Modo)}, @{nameof(Gasto.Importe)}, @{nameof(Gasto.Anulado)}, @{nameof(Gasto.NombreGasto)});           
                        SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Gasto.Fecha)}", gasto.Fecha);
                command.Parameters.AddWithValue($"@{nameof(Gasto.Modo)}", gasto.Modo);
                command.Parameters.AddWithValue($"@{nameof(Gasto.Importe)}", gasto.Importe);
                command.Parameters.AddWithValue($"@{nameof(Gasto.Anulado)}", "NO");
                command.Parameters.AddWithValue($"@{nameof(Gasto.NombreGasto)}", gasto.NombreGasto);
                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                gasto.Id = id;
                connection.Close();
            }
        }
        return id;
    }

    public Gasto? GetGasto(int id)
    {
        Gasto? gastos = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Gasto.Id)},{nameof(Gasto.Fecha)}, {nameof(Gasto.Modo)}, {nameof(Gasto.Importe)}, {nameof(Gasto.Anulado)}, {nameof(Gasto.NombreGasto)}
                FROM gastos p
                WHERE p.{nameof(Gasto.Id)} = @{nameof(Gasto.Id)};";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Gasto.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        gastos = new Gasto
                        {
                            Id = reader.GetInt32(reader.GetOrdinal(nameof(Gasto.Id))),
                            Fecha = reader.GetDateTime(reader.GetOrdinal(nameof(Gasto.Fecha))),
                            Modo = reader.GetString(reader.GetOrdinal(nameof(Gasto.Modo))),
                            Importe = reader.GetDouble(reader.GetOrdinal(nameof(Gasto.Importe))),
                            Anulado = reader.GetString(reader.GetOrdinal(nameof(Gasto.Anulado))),
                            NombreGasto = reader.GetString(reader.GetOrdinal(nameof(Gasto.NombreGasto)))
                        };
                    }
                }
            }
        }
        return gastos;
    }
    

    public int ModificarPago(Gasto gasto)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"UPDATE gastos SET 
                            {nameof(Gasto.Fecha)} = @{nameof(Gasto.Fecha)},
                            {nameof(Gasto.Modo)} = @{nameof(Gasto.Modo)},
                            {nameof(Gasto.Importe)} = @{nameof(Gasto.Importe)},
                            {nameof(Gasto.NombreGasto)} = @{nameof(Gasto.NombreGasto)}
                        WHERE {nameof(Gasto.Id)} = @{nameof(Gasto.Id)};";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Gasto.Fecha)}", gasto.Fecha);
                command.Parameters.AddWithValue($"@{nameof(Gasto.Modo)}", gasto.Modo);
                command.Parameters.AddWithValue($"@{nameof(Gasto.Importe)}", gasto.Importe);
                command.Parameters.AddWithValue($"@{nameof(Gasto.NombreGasto)}", gasto.NombreGasto);
                command.Parameters.AddWithValue($"@{nameof(Gasto.Id)}", gasto.Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }


    public int EliminarPago(int id) //Es un anulado lógico
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE gastos SET {nameof(Gasto.Anulado)} = 'SI'  WHERE {nameof(Gasto.Id)} = @id;";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Gasto.Id)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

}