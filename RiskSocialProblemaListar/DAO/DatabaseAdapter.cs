using MySql.Data.MySqlClient;
using RiskSocialProblemaListar.Domain;
using RiskSocialProblemaListar.Interfaces;
using System.Data;

namespace RiskSocialProblemaListar.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Response ProblemaListar(Request request)
        {
            Response response = new();
            response.Problemas = new List<Problema>();

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_problemas_listar";

                    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;

                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            Problema data = new()
                            {
                                NombreProblema = (cursor["VC_NOMBRE_PROBLEMA"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE_PROBLEMA"]),
                                Descripcion = (cursor["VC_DESCRIPCION_PROBLEMA"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_DESCRIPCION_PROBLEMA"]),
                                Pioridad = (cursor["VC_PRIORIDAD"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_PRIORIDAD"]),
                                Criticidad = (cursor["VC_CRITICIDAD"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_CRITICIDAD"]),
                                Estado = (cursor["VC_ESTADO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_ESTADO"]),
                                NombreUsuarioAsignado = (cursor["VC_NOMBRE"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE"]),
                                NombreRiesgo = (cursor["VC_NOMBRE_RIESGO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE_RIESGO"]),
                            };

                            response.Problemas.Add(data);
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
