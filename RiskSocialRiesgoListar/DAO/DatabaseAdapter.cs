using MySql.Data.MySqlClient;
using RiskSocialRiesgoListar.Domain;
using RiskSocialRiesgoListar.Interfaces;
using System.Data;

namespace RiskSocialRiesgoListar.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Response RiesgoListar(Request request)
        {
            Response response = new();
            response.Riesgos = new List<Riesgo>();

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_riesgos_listar";

                    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;

                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            Riesgo data = new()
                            {
                                NombreRiesgo = (cursor["VC_NOMBRE_RIESGO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE_RIESGO"]),
                                Probabilidad = (cursor["DO_PROBABILIDAD"] == DBNull.Value) ? null : Convert.ToDouble(cursor["DO_PROBABILIDAD"]),
                                Impacto = (cursor["DO_IMPACTO"] == DBNull.Value) ? null : Convert.ToDouble(cursor["DO_IMPACTO"]),
                                Prioridad = (cursor["VC_PRIODIDAD"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_PRIODIDAD"]),
                                Criticidad = (cursor["VC_CRITICIDAD"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_CRITICIDAD"]),
                                Estado = (cursor["VC_ESTADO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_ESTADO"]),
                                Usuario = (cursor["VC_USUARIO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_USUARIO"]),
                            };

                            response.Riesgos.Add(data);
                        }
                    }
                    cn.Close();

                }
            }
            catch (Exception)
            {
            }


            return response;
        }
    }
}
