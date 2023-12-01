using MySql.Data.MySqlClient;
using RiskSocialUsuariosListar.Domain;
using RiskSocialUsuariosListar.Interfaces;
using System.Data;

namespace RiskSocialUsuariosListar.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Response UsuarioListar(Request request)
        {
            Response response = new();
            response.Usuarios = new List<Usuario>();

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_usuario_listar";

                    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;

                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            Usuario data = new()
                            {
                                IdUsuario = (cursor["IN_ID_USUARIO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["IN_ID_USUARIO"]),
                                NombreUsuario = (cursor["VC_USUARIO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_USUARIO"]),
                            };

                            response.Usuarios.Add(data);
                        }
                    }
                    cn.Close();

                }
            }
            catch (Exception)
            {
            }


            return response;
        }
    }
}
