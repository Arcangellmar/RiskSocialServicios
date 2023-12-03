using MySql.Data.MySqlClient;
using RiskSocialAccionCrear.Domain;
using RiskSocialAccionCrear.Interfaces;
using System.Data;

namespace RiskSocialAccionCrear.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int? AccionCrear(Request request)
        {
            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_accion_crear";

                    cmd.Parameters.Add("@PARAM_VC_NOMBRE_ACCION", MySqlDbType.VarChar).Value = request.NombreAccion;
                    cmd.Parameters.Add("@PARAM_VC_DESCRIPCION", MySqlDbType.VarChar).Value = request.DescripcionAccion;
                    cmd.Parameters.Add("@PARAM_IN_ID_USUARIO_CREADOR", MySqlDbType.Int32).Value = request.IdUsuarioCreador;
                    cmd.Parameters.Add("@PARAM_IN_ID_USUARIO_ASIGNADO", MySqlDbType.Int32).Value = request.IdUsuarioAsignado;
                    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;
                    cmd.Parameters.Add("@PARAM_VC_ESTADO", MySqlDbType.VarChar).Value = request.Estado;

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
