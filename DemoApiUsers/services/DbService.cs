using DemoApiUsers.models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApiUsers.services
{
    public class DbService : IDbService
    {
        private SqlConnection _connection;

        public DbService()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connString = config.GetConnectionString("Default");

            _connection = new SqlConnection(connString);
        }

        public async Task<ResponseBase<bool>> EliminarUsuario(int id)
        {
            var resultado = false;
            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"update usuario set activo=0 where idusuario={id}";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.Text;
                    var reader = await commado.ExecuteNonQueryAsync();
                    if (reader > 0)
                        resultado = true;
                }

                return new ResponseBase<bool> { TieneResultado= true, Mensaje="Borrado con Exito", Modelo = resultado };

            }
            catch(Exception e)
            {
                return new ResponseBase<bool> { TieneResultado = false, Mensaje = "Error Interno, Consulte al Administrador del sistema", Modelo = resultado };
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<ResponseBase<Usuario>> GuardarUsuario(Usuario modelo)
        {
            try
            {
                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"";

                    if (modelo.IdUsuario==0)
                    {
                        sql = $"insert into usuario values ('{modelo.nombredeusuario}','{modelo.nombres}','{modelo.apellidos}','{modelo.contrasenia}',1); select scope_identity();";
                    }
                    else
                    {
                        sql = $"update usuario set nombredeusuario='{modelo.nombredeusuario}',nombres='{modelo.nombres}', apellidos='{modelo.apellidos}', contrasenia='{modelo.contrasenia}' where idusuario={modelo.IdUsuario}; select {modelo.IdUsuario};";
                    }

                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.Text;
                    var reader = await commado.ExecuteScalarAsync();

                    modelo.IdUsuario = int.Parse(reader.ToString());
                  
                }

                return new ResponseBase<Usuario> { TieneResultado = true, Mensaje = "Usuario guardado correctamente", Modelo = modelo };

            }
            catch (Exception e)
            {
                return new ResponseBase<Usuario> { TieneResultado = false, Mensaje = "Error Interno, Consulte al Administrador del sistema", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<ResponseBase<IEnumerable<Usuario>>> ObtenerUsuario()
        {
            try
            {
                var listaUsuarios = new List<Usuario>();

                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"select * from usuario where activo=1";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.Text;
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        var usuario = new Usuario();
                        usuario.IdUsuario = int.Parse(reader["IdUsuario"].ToString());
                        usuario.nombredeusuario = reader["nombredeusuario"].ToString();
                        usuario.nombres = reader["nombres"].ToString();
                        usuario.apellidos = reader["apellidos"].ToString();
                        usuario.contrasenia = reader["contrasenia"].ToString();
                        usuario.Activo = bool.Parse(reader["Activo"].ToString());
                        listaUsuarios.Add(usuario);
                    }
                  
                }

                return new ResponseBase<IEnumerable<Usuario>> { TieneResultado = true, Mensaje = "Lista obtenida correctamente", Modelo = listaUsuarios };

            }
            catch (Exception e)
            {
                return new ResponseBase<IEnumerable<Usuario>> { TieneResultado = false, Mensaje = "Error Interno, Consulte al Administrador del sistema", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<ResponseBase<Usuario>> ObtenerUsuarioPorId(int id)
        {
            try
            {
                var usuario = new Usuario();

                _connection.Open();

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    string sql = $"select * from usuario where idusuario={id}";
                    SqlCommand commado = new SqlCommand(sql, _connection);
                    commado.CommandType = System.Data.CommandType.Text;
                    var reader = await commado.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        usuario = new Usuario();
                        usuario.IdUsuario = int.Parse(reader["IdUsuario"].ToString());
                        usuario.nombredeusuario = reader["nombredeusuario"].ToString();
                        usuario.nombres = reader["nombres"].ToString();
                        usuario.apellidos = reader["apellidos"].ToString();
                        usuario.contrasenia = reader["contrasenia"].ToString();
                        usuario.Activo = bool.Parse(reader["Activo"].ToString());                       
                    }

                }

                return new ResponseBase<Usuario> { TieneResultado = true, Mensaje = "Lista obtenida correctamente", Modelo = usuario };

            }
            catch (Exception e)
            {
                return new ResponseBase<Usuario> { TieneResultado = false, Mensaje = "Error Interno, Consulte al Administrador del sistema", Modelo = null };
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
