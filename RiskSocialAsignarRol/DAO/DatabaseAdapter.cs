using MySql.Data.MySqlClient;
using RiskSocialAsignarRol.Domain;
using RiskSocialAsignarRol.Interfaces;
using System.Data;

namespace RiskSocialAsignarRol.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //public bool RolCambiar(Request request)
        //{
        //    try
        //    {
        //        using (MySqlConnection cn = new(connectionString))
        //        {
        //            MySqlCommand cmd = new();
        //            cn.Open();
        //            cmd.Connection = cn;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = @"sp_rol_cambiar";

        //            cmd.Parameters.Add("@PARAM_IN_ID_USUARIO", MySqlDbType.Int32).Value = request.IdUsuario;
        //            cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;
        //            cmd.Parameters.Add("@PARAM_IN_ID_ROL", MySqlDbType.Int32).Value = request.IdRol;

        //            string? ReturnBD = cmd.ExecuteScalar().ToString();
        //            cn.Close();

        //            return ReturnBD == "1";

        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        public bool RolCambiar(Request request)
        {
            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_rol_insertar";

                    cmd.Parameters.Add("@PARAM_IN_ID_USUARIO", MySqlDbType.Int32).Value = request.IdUsuario;
                    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;
                    cmd.Parameters.Add("@PARAM_IN_ID_ROL", MySqlDbType.Int32).Value = request.IdRol;

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
