using MySql.Data.MySqlClient;
using RiskSocialProblemaCrear.Domain;
using RiskSocialProblemaCrear.Interfaces;
using System.Data;

namespace RiskSocialProblemaCrear.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int? ProblemaCrear(Request request)
        {
            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_problema_crear";

                    cmd.Parameters.Add("@PARAM_VC_NOMBRE_PROBLEMA", MySqlDbType.VarChar).Value = request.NombreProblema;
                    cmd.Parameters.Add("@PARAM_VC_DESCRIPCION_PROBLEMA", MySqlDbType.VarChar).Value = request.DescripcionProblema;
                    cmd.Parameters.Add("@PARAM_VC_PRIODIDAD", MySqlDbType.VarChar).Value = request.Prioricidad;
                    cmd.Parameters.Add("@PARAM_VC_CRITICIDAD", MySqlDbType.VarChar).Value = request.Criticidad;
                    cmd.Parameters.Add("@PARAM_IN_ID_USUARIO_ASIGNADO", MySqlDbType.Int32).Value = request.UsuarioAsignado;
                    cmd.Parameters.Add("@PARAM_IN_ID_RIESGO", MySqlDbType.Int32).Value = request.IdRiesgo;

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
