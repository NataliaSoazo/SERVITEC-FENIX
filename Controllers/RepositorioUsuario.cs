using MySql.Data.MySqlClient;
using SERVITEC_FENIX.Models;


namespace SERVITEC_FENIX.Models;

public class RepositorioUsuario
{
    readonly string ConnectionString = "Server=localhost;Database=servitec;User=root;Password=;";

    public RepositorioUsuario()
    {

    }

    public List<Usuario> ObtenerUsuarios()
    {
        var usuarios = new List<Usuario>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = $"SELECT {nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Correo)}, {nameof(Usuario.Clave)}, {nameof(Usuario.Rol)} FROM usuarios";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Correo = reader.GetString(nameof(Usuario.Correo)),
                            Clave = reader.GetString(nameof(Usuario.Clave)),
                            Rol = reader.GetString(nameof(Usuario.Rol)),
                           
                        });
                    }

                }
            }
        }
        return usuarios;
    }


    public Usuario ValidarUsuario(string c, string cl)
    {
        //return ObtenerUsuarios().Where(item => item.Correo == c && item.Clave == cl).FirstOrDefault();
           return getUsuario(c);
           
           
    }

    public Usuario getUsuario(string correo)
{
    Usuario usuario = null;
    using (var connection = new MySqlConnection(ConnectionString))
    {
        var sql = @$"SELECT {nameof(Usuario.Id)},{nameof(Usuario.Nombre)},{nameof(Usuario.Apellido)},{nameof(Usuario.Correo)},{nameof(Usuario.Clave)},{nameof(Usuario.Rol)}
        FROM usuarios WHERE {nameof(Usuario.Correo)} = @{nameof(Usuario.Correo)}";
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue($"@{nameof(Usuario.Correo)}", correo);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32(nameof(Usuario.Id)),
                        Nombre = reader.GetString(nameof(Usuario.Nombre)),
                        Apellido = reader.GetString(nameof(Usuario.Apellido)),
                        Correo = reader.GetString(nameof(Usuario.Correo)),
                        Clave = reader.GetString(nameof(Usuario.Clave)),
                        Rol = reader.GetString(nameof(Usuario.Rol))
                    };
                }
            }
        }
    }
    return usuario;
}
    




    /* public int AltaPropietario(Propietario propietario){
         int id = 0;
         using (var connection = new MySqlConnection(ConnectionString)){
             var sql = @$"INSERT INTO propietarios ({nameof(Propietario.Nombre)},{nameof(Propietario.Apellido)},{nameof(Propietario.Dni)},{nameof(Propietario.Email)},{nameof(Propietario.Telefono)},{nameof(Propietario.Domicilio)},{nameof(Propietario.Ciudad)})
                                             VALUES (@{nameof(Propietario.Nombre)},@{nameof(Propietario.Apellido)},@{nameof(Propietario.Dni)},@{nameof(Propietario.Email)},@{nameof(Propietario.Telefono)},@{nameof(Propietario.Domicilio)},@{nameof(Propietario.Ciudad)});            
              SELECT LAST_INSERT_ID();";
             using (var command = new MySqlCommand(sql, connection)){
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", propietario.Nombre);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", propietario.Apellido);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Dni)}", propietario.Dni);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Email)}", propietario.Email);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", propietario.Telefono);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Domicilio)}", propietario.Domicilio);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Ciudad)}", propietario.Ciudad);

                 connection.Open();
                 id = Convert.ToInt32(command.ExecuteScalar());
                 propietario.Id = id;
                 connection.Close();


             }
         }
         return id;
     }

     

     public int ModificarPropietario(Propietario propietario){
         using (var connection = new MySqlConnection(ConnectionString)){
             var sql = @$"UPDATE propietarios 
             SET {nameof(Propietario.Nombre)} = @{nameof(Propietario.Nombre)},
             {nameof(Propietario.Apellido)} = @{nameof(Propietario.Apellido)},
             {nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)},
             {nameof(Propietario.Email)} = @{nameof(Propietario.Email)},
             {nameof(Propietario.Telefono)} = @{nameof(Propietario.Telefono)},
             {nameof(Propietario.Domicilio)} = @{nameof(Propietario.Domicilio)},
             {nameof(Propietario.Ciudad)} = @{nameof(Propietario.Ciudad)}
             WHERE {nameof(Propietario.Id)} = @{nameof(Propietario.Id)} ";     
             using (var command = new MySqlCommand(sql, connection)){
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Id)}", propietario.Id);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", propietario.Nombre);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", propietario.Apellido);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Dni)}", propietario.Dni);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Email)}", propietario.Email);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", propietario.Telefono);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Domicilio)}", propietario.Domicilio);
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Ciudad)}", propietario.Ciudad);

                 connection.Open();
                 command.ExecuteNonQuery();
                 connection.Close();
             }
         }
         return 0;
     }

     public int  EliminarPersona(int id){
         using(var connection = new MySqlConnection(ConnectionString))
         {
             var sql = @$"DELETE from propietarios WHERE {nameof(Propietario.Id)} = @{nameof(Propietario.Id)}";
             using (var command = new MySqlCommand(sql, connection))
             {
                 command.Parameters.AddWithValue($"@{nameof(Propietario.Id)}", id);
                 connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
             }
         }
          return 0;
     }*/

}