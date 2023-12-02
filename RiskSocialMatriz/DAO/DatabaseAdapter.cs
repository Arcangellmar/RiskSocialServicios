using MySql.Data.MySqlClient;
using RiskSocialMatriz.Domain;
using RiskSocialMatriz.Interfaces;
using System;
using System.Data;

namespace RiskSocialMatriz.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Response? Matriz(Request request)
        {
            Response response = new Response();

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_matriz";

                    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;

                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            response = new()
                            {
                                MuyPosibleMuyBajo = (cursor["MUY_POSIBLE_MUY_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["MUY_POSIBLE_MUY_BAJO"]),
                                MuyPosibleBajo = (cursor["MUY_POSIBLE_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["MUY_POSIBLE_BAJO"]),
                                MuyPosibleMedio = (cursor["MUY_POSIBLE_MEDIO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["MUY_POSIBLE_MEDIO"]),
                                MuyPosibleAlto = (cursor["MUY_POSIBLE_ALTO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["MUY_POSIBLE_ALTO"]),
                                MuyPosibleCritico = (cursor["MUY_POSIBLE_CRITICO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["MUY_POSIBLE_CRITICO"]),

                                PosibleMuyBajo = (cursor["POSIBLE_MUY_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["POSIBLE_MUY_BAJO"]),
                                PosibleBajo = (cursor["POSIBLE_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["POSIBLE_BAJO"]),
                                PosibleMedio = (cursor["POSIBLE_MEDIO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["POSIBLE_MEDIO"]),
                                PosibleAlto = (cursor["POSIBLE_ALTO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["POSIBLE_ALTO"]),
                                PosibleCritico = (cursor["POSIBLE_CRITICO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["POSIBLE_CRITICO"]),

                                OcasionalMuyBajo = (cursor["OCASIONAL_MUY_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["OCASIONAL_MUY_BAJO"]),
                                OcasionalBajo = (cursor["OCASIONAL_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["OCASIONAL_BAJO"]),
                                OcasionalMedio = (cursor["OCASIONAL_MEDIO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["OCASIONAL_MEDIO"]),
                                OcasionalAlto = (cursor["OCASIONAL_ALTO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["OCASIONAL_ALTO"]),
                                OcasionalCritico = (cursor["OCASIONAL_CRITICO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["OCASIONAL_CRITICO"]),

                                ProbableMuyBajo = (cursor["PROBABLE_MUY_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["PROBABLE_MUY_BAJO"]),
                                ProbableBajo = (cursor["PROBABLE_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["PROBABLE_BAJO"]),
                                ProbableMedio = (cursor["PROBABLE_MEDIO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["PROBABLE_MEDIO"]),
                                ProbableAlto = (cursor["PROBABLE_ALTO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["PROBABLE_ALTO"]),
                                ProbableCritico = (cursor["PROBABLE_CRITICO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["PROBABLE_CRITICO"]),

                                ImprobableMuyBajo = (cursor["IMPROBABLE_MUY_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["IMPROBABLE_MUY_BAJO"]),
                                ImprobableBajo = (cursor["IMPROBABLE_BAJO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["IMPROBABLE_BAJO"]),
                                ImprobableMedio = (cursor["IMPROBABLE_MEDIO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["IMPROBABLE_MEDIO"]),
                                ImprobableAlto = (cursor["IMPROBABLE_ALTO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["IMPROBABLE_ALTO"]),
                                ImprobableCritico = (cursor["IMPROBABLE_CRITICO"] == DBNull.Value) ? 0 : Convert.ToInt32(cursor["IMPROBABLE_CRITICO"]),
                            };
                        }

                        if (cursor.NextResult())
                        {
                            response.Riesgos = new List<Riesgo>();

                            while (cursor.Read())
                            {
                                Riesgo riesgo = new Riesgo
                                {
                                    IdRiesgo = (cursor["IN_ID_RIESGO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["IN_ID_RIESGO"]),
                                    NombreRiesgo = (cursor["VC_NOMBRE_RIESGO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE_RIESGO"]),
                                    Probabilidad = (cursor["DO_PROBABILIDAD"] == DBNull.Value) ? null : Convert.ToDouble(cursor["DO_PROBABILIDAD"]),
                                    Impacto = (cursor["DO_IMPACTO"] == DBNull.Value) ? null : Convert.ToDouble(cursor["DO_IMPACTO"]),
                                    Prioridad = (cursor["VC_PRIODIDAD"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_PRIODIDAD"]),
                                    Criticidad = (cursor["VC_CRITICIDAD"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_CRITICIDAD"]),
                                    Estado = (cursor["VC_ESTADO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_ESTADO"]),
                                    Usuario = (cursor["VC_USUARIO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_USUARIO"]),
                                };
                                response.Riesgos.Add(riesgo);
                            }
                        }

                    }
                    cn.Close();

                    return response;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
