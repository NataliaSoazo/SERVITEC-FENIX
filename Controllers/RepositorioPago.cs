using MySql.Data.MySqlClient;

namespace SERVITEC_FENIX.Models;

public class RepositorioPago
{
    readonly string ConnectionString = "Server=localhost;Database=servitec;User=root;Password=;";

    public RepositorioPago()
    {

    }

    public IList<Pago> GetPagos()
    {
        var pagos = new List<Pago>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Pago.Id)},{nameof(Pago.Reparacion)}, {nameof(Pago.Fecha)}, {nameof(Pago.Modo)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}
                FROM pagos p  ORDER BY p.{nameof(Pago.Fecha)} DESC";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pagos.Add(new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            Reparacion = reader.GetString(nameof(Pago.Reparacion)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Modo = reader.GetString(nameof(Pago.Modo)),
                            Importe = reader.GetDouble(nameof(Pago.Importe)),
                            Anulado = reader.GetString(nameof(Pago.Anulado))
                        });
                    }

                }
            }
        }
        return pagos;
    }
    public IList<Pago> ObtenerPagosPorReparacion(int id)
    {
        var pagos = new List<Pago>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Pago.Id)},  {nameof(Pago.Fecha)}, {nameof(Pago.Modo)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.Reparacion)}
                FROM pagos p
                 WHERE p.{nameof(Pago.Reparacion)} = @{nameof(Pago.Reparacion)};";
            using (var command = new MySqlCommand(sql, connection))
            {   command.Parameters.AddWithValue($"@{nameof(Pago.Reparacion)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pagos.Add(new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Modo = reader.GetString(nameof(Pago.Modo)),
                            Importe = reader.GetDouble(nameof(Pago.Importe)),
                            Anulado = reader.GetString(nameof(Pago.Anulado)),
                            Reparacion = reader.GetString(nameof(Pago.Reparacion)),
                        });
                    }

                }
            }
        }
        return pagos;
    }

    public int AltaPago(Pago pago)
    {
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"INSERT INTO pagos ({nameof(Pago.Fecha)}, {nameof(Pago.Modo)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.Reparacion)}) 
                        VALUES (@{nameof(Pago.Fecha)}, @{nameof(Pago.Modo)}, @{nameof(Pago.Importe)}, @{nameof(Pago.Anulado)}, @{nameof(Pago.Reparacion)});           
                        SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Fecha)}", pago.Fecha);
                command.Parameters.AddWithValue($"@{nameof(Pago.Modo)}", pago.Modo);
                command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"@{nameof(Pago.Anulado)}", "NO");
                command.Parameters.AddWithValue($"@{nameof(Pago.Reparacion)}", pago.Reparacion);
                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                pago.Id = id;
                connection.Close();
            }
        }
        return id;
    }

    public Pago? GetPago(int id)
    {
        Pago? pagos = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Pago.Id)},{nameof(Pago.Fecha)}, {nameof(Pago.Modo)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.Reparacion)}
                FROM pagos p
                WHERE p.{nameof(Pago.Id)} = @{nameof(Pago.Id)};";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pagos = new Pago
                        {
                            Id = reader.GetInt32(reader.GetOrdinal(nameof(Pago.Id))),
                            Fecha = reader.GetDateTime(reader.GetOrdinal(nameof(Pago.Fecha))),
                            Modo = reader.GetString(reader.GetOrdinal(nameof(Pago.Modo))),
                            Importe = reader.GetDouble(reader.GetOrdinal(nameof(Pago.Importe))),
                            Anulado = reader.GetString(reader.GetOrdinal(nameof(Pago.Anulado))),
                            Reparacion = reader.GetString(reader.GetOrdinal(nameof(Pago.Reparacion)))
                        };
                    }
                }
            }
        }
        return pagos;
    }
    

    public int ModificarPago(Pago pago)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"UPDATE pagos SET 
                            {nameof(Pago.Fecha)} = @{nameof(Pago.Fecha)},
                            {nameof(Pago.Modo)} = @{nameof(Pago.Modo)},
                            {nameof(Pago.Importe)} = @{nameof(Pago.Importe)},
                            {nameof(Pago.Reparacion)} = @{nameof(Pago.Reparacion)}
                        WHERE {nameof(Pago.Id)} = @{nameof(Pago.Id)};";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Fecha)}", pago.Fecha);
                command.Parameters.AddWithValue($"@{nameof(Pago.Modo)}", pago.Modo);
                command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"@{nameof(Pago.Reparacion)}", pago.Reparacion);
                command.Parameters.AddWithValue($"@{nameof(Pago.Id)}", pago.Id);

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
            var sql = @$"UPDATE pagos SET {nameof(Pago.Anulado)} = 'SI'  WHERE {nameof(Pago.Id)} = @id;";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Id)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

}