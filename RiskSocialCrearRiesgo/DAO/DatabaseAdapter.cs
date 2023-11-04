using MySql.Data.MySqlClient;
using RiskSocialCrearRiesgo.Domain;
using RiskSocialCrearRiesgo.Interfaces;
using System.Data;

namespace RiskSocialCrearRiesgo.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int? RiesgoCrear(Request request)
        {
            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_riesgo_crear";

                    cmd.Parameters.Add("@PARAM_VC_NOMBRE_RIESGO", MySqlDbType.VarChar).Value = request.NombreRiesgo;
                    cmd.Parameters.Add("@PARAM_VC_DESCRIPCION_RIESGO", MySqlDbType.VarChar).Value = request.DescripcionRiesgo;
                    cmd.Parameters.Add("@PARAM_DO_PROBABILIDAD", MySqlDbType.Double).Value = request.Probabilidad;
                    cmd.Parameters.Add("@PARAM_DO_IMPACTO", MySqlDbType.Double).Value = request.Impacto;
                    cmd.Parameters.Add("@PARAM_VC_PRIODIDAD", MySqlDbType.VarChar).Value = request.Prioricidad;
                    cmd.Parameters.Add("@PARAM_VC_CRITICIDAD", MySqlDbType.VarChar).Value = request.Criticidad;
                    cmd.Parameters.Add("@PARAM_IN_ID_USUARIO_ASIGNADO", MySqlDbType.Int32).Value = request.UsuarioAsignado;
                    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;

                    string? ReturnBD = cmd.ExecuteScalar().ToString();
                    cn.Close();

                    return int.Parse(ReturnBD);

                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
