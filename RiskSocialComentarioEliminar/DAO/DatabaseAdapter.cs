using MySql.Data.MySqlClient;
using RiskSocialComentarioEliminar.Domain;
using RiskSocialComentarioEliminar.Interfaces;
using System.Data;

namespace RiskSocialComentarioEliminar.DAO
{

    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool ComentarioEliminar(Request request)
        {

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_comentario_eliminar";

                    cmd.Parameters.Add("@PARAM_ID_COMENTARIO", MySqlDbType.Int32).Value = request.IdComentario;

                    string? ReturnBD = cmd.ExecuteScalar().ToString();
                    cn.Close();

                    return ReturnBD == "1";

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
