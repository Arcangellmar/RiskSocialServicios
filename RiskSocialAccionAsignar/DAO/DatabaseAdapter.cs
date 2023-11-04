using MySql.Data.MySqlClient;
using RiskSocialAccionAsignar.Domain;
using RiskSocialAccionAsignar.Interfaces;
using System.Data;

namespace RiskSocialAccionAsignar.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool AccionActualizar(Request request)
        {
            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_accion_actualizar_responsable";

                    cmd.Parameters.Add("@PARAM_IN_ID_USUARIO_RESPONSABLE", MySqlDbType.Int32).Value = request.IdUsuario;
                    cmd.Parameters.Add("@PARAM_IN_ID_ACCION_MITIGAR", MySqlDbType.Int32).Value = request.IdAccion;

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
