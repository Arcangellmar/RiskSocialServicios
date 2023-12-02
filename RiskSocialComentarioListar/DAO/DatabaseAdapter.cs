using MySql.Data.MySqlClient;
using RiskSocialComentarioListar.Domain;
using RiskSocialComentarioListar.Interfaces;
using System.Data;

namespace RiskSocialComentarioListar.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Response ComentarioListar(Request request)
        {
            Response response = new();
            response.Comentarios = new List<Comentarios>();

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_comentarios_listar";

                    cmd.Parameters.Add("@PARAM_IN_ID_REPORTE", MySqlDbType.Int32).Value = request.IdReporte;
                    cmd.Parameters.Add("@PARAM_IN_ID_RIESGO", MySqlDbType.Int32).Value = request.IdRiesgo;
                    cmd.Parameters.Add("@PARAM_IN_ID_ACCION", MySqlDbType.Int32).Value = request.IdAccion;
                    cmd.Parameters.Add("@PARAM_IN_ID_PROBLEMA", MySqlDbType.Int32).Value = request.IdProblema;

                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            Comentarios data = new()
                            {
                                NombreUsuario = (cursor["VC_NOMBRE"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE"]),
                                Comentario = (cursor["VC_COMENTARIO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_COMENTARIO"]),
                            };

                            response.Comentarios.Add(data);
                        }
                    }
                    cn.Close();

                }
            }
            catch (Exception ex)
            {
            }


            return response;
        }
    }
}
