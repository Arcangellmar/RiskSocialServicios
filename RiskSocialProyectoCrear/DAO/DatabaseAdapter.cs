using MySql.Data.MySqlClient;
using RiskSocialProyectoCrear.Domain;
using RiskSocialProyectoCrear.Interfaces;
using System.Data;

namespace RiskSocialProyectoCrear.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int? ProyectoCrear(Request request)
        {
            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_proyecto_crear";

                    cmd.Parameters.Add("@PARAM_VC_NOMBRE_PROYECTO", MySqlDbType.VarChar).Value = request.NombreProyecto;
                    cmd.Parameters.Add("@PARAM_VC_DESCRIPCION_PROYECTO", MySqlDbType.VarChar).Value = request.DescripcionProyecto;
                    cmd.Parameters.Add("@PARAM_IN_USUARIO_RESPONSABLE", MySqlDbType.Int32).Value = request.UsuarioResponsable;
                    cmd.Parameters.Add("@PARAM_DT_FECHA_INICIO", MySqlDbType.DateTime).Value = request.FechaInicio;
                    cmd.Parameters.Add("@PARAM_DT_FECHA_FIN", MySqlDbType.DateTime).Value = request.FechaFin;

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

        public bool ProyectoUsuarioCrear(int? IdUsuario, int? IdProyecto, int? IdRol)
        {
            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_proyecto_usuario_crear";

                    cmd.Parameters.Add("@PARAM_IN_ID_USUARIO", MySqlDbType.Int32).Value = IdUsuario;
                    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = IdProyecto;
                    cmd.Parameters.Add("@PARAM_IN_ID_ROL", MySqlDbType.Int32).Value = IdRol;

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
