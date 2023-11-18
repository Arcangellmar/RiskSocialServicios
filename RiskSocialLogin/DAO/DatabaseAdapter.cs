using MySql.Data.MySqlClient;
using RiskSocialLogin.Domain;
using RiskSocialLogin.Interfaces;
using RiskSocialLogin.Utils;
using System.Data;

namespace RiskSocialLogin.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Response? Login(Request request)
        {
            try
            {
                Response? response = null;

                request.Pass = Encry.HashPassword(request.Pass);

                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_usuario_login";

                    cmd.Parameters.Add("@PARAM_VC_CORREO", MySqlDbType.VarChar).Value = request.Correo;
                    cmd.Parameters.Add("@PARAM_VC_PASS", MySqlDbType.VarChar).Value = request.Pass;

                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            response = new()
                            {
                                IdUsuario = (cursor["IN_ID_USUARIO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["IN_ID_USUARIO"]),
                                Usuario = (cursor["VC_USUARIO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_USUARIO"]),
                                Nombre = (cursor["VC_NOMBRE"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE"]),
                                Correo = (cursor["VC_CORREO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_CORREO"])
                            };
                        }
                    }

                    return response;

                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
