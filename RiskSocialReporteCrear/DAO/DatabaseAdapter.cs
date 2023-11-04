using MySql.Data.MySqlClient;
using RiskSocialReporteCrear.Domain;
using RiskSocialReporteCrear.Interfaces;
using System.Data;

namespace RiskSocialReporteCrear.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int? ReporteCrear(Request request)
        {
            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_reporte_crear";

                    cmd.Parameters.Add("@PARAM_VC_CONTENIDO_REPORTE", MySqlDbType.VarChar).Value = request.ContenidoReporte;
                    cmd.Parameters.Add("@PARAM_IN_USUARIO_CREADOR", MySqlDbType.Int32).Value = request.IdUsuarioCreador;
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
