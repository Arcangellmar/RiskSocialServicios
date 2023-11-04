using MySql.Data.MySqlClient;
using RiskSocialCrearUsuario.Domain;
using RiskSocialCrearUsuario.Interfaces;
using System.Data;

namespace RiskSocialCrearUsuario.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool UsuarioCrear(Request request)
        {

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_usuario_crear";

                    cmd.Parameters.Add("@PARAM_VC_USUARIO", MySqlDbType.VarChar).Value = request.Usuario;
                    cmd.Parameters.Add("@PARAM_VC_PASS", MySqlDbType.VarChar).Value = request.Pass;
                    cmd.Parameters.Add("@PARAM_VC_NOMBRE", MySqlDbType.VarChar).Value = request.Nombre;
                    cmd.Parameters.Add("@PARAM_VC_CORREO", MySqlDbType.VarChar).Value = request.Correo;

                    string? ReturnBD = cmd.ExecuteScalar().ToString();
                    cn.Close();

                    return ReturnBD == "1";

                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}
