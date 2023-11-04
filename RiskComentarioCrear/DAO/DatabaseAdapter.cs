using MySql.Data.MySqlClient;
using RiskComentarioCrear.Domain;
using RiskComentarioCrear.Interfaces;
using System.Data;

namespace RiskComentarioCrear.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int? ComentarioCrear(Request request)
        {
            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_comentario_crear";

                    cmd.Parameters.Add("@PARAM_IN_ID_USUARIO", MySqlDbType.Int32).Value = request.IdUsuarioCreador;
                    cmd.Parameters.Add("@PARAM_VC_COMENTARIO", MySqlDbType.VarChar).Value = request.Comentario;
                    cmd.Parameters.Add("@PARAM_IN_ID_REPORTE", MySqlDbType.Int32).Value = request.IdReporte;
                    cmd.Parameters.Add("@PARAM_IN_ID_RIESGO", MySqlDbType.Int32).Value = request.IdRiesgo;
                    cmd.Parameters.Add("@PARAM_IN_ID_ACCION", MySqlDbType.Int32).Value = request.IdAccion;
                    cmd.Parameters.Add("@PARAM_IN_ID_PROBLEMA", MySqlDbType.Int32).Value = request.IdProblema;

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
