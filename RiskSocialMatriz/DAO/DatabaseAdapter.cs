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
            Random random = new Random();
            Response response = new Response
            {
                MuyPosibleMuyBajo = random.Next(1, 6),
                MuyPosibleBajo = random.Next(1, 6),
                MuyPosibleMedio = random.Next(1, 6),
                MuyPosibleAlto = random.Next(1, 6),
                MuyPosibleCritico = random.Next(1, 6),

                PosibleMuyBajo = random.Next(1, 6),
                PosibleBajo = random.Next(1, 6),
                PosibleMedio = random.Next(1, 6),
                PosibleAlto = random.Next(1, 6),
                PosibleCritico = random.Next(1, 6),

                OcasionalMuyBajo = random.Next(1, 6),
                OcasionalBajo = random.Next(1, 6),
                OcasionalMedio = random.Next(1, 6),
                OcasionalAlto = random.Next(1, 6),
                OcasionalCritico = random.Next(1, 6),

                ProbableMuyBajo = random.Next(1, 6),
                ProbableBajo = random.Next(1, 6),
                ProbableMedio = random.Next(1, 6),
                ProbableAlto = random.Next(1, 6),
                ProbableCritico = random.Next(1, 6),

                ImprobableMuyBajo = random.Next(1, 6),
                ImprobableBajo = random.Next(1, 6),
                ImprobableMedio = random.Next(1, 6),
                ImprobableAlto = random.Next(1, 6),
                ImprobableCritico = random.Next(1, 6),

            };

            response.Riesgos = new List<Riesgo>();

            for (int i = 0; i < 6; i++)
            {
                Riesgo riesgo = new Riesgo
                {
                    IdRiesgo = 0,
                    NombreRiesgo = "Text",
                    Probabilidad = 1,
                    Impacto = 1,
                    Prioridad = "Alta",
                    Criticidad = "Alta",
                    Estado = "En proceso",
                    Usuario = "Rolando"
                };

                response.Riesgos.Add(riesgo);
            }

            return response;
            try
            {
                //using (MySqlConnection cn = new(connectionString))
                //{
                //    MySqlCommand cmd = new();
                //    cn.Open();
                //    cmd.Connection = cn;
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandText = @"sp_problema_crear";

                //    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;

                //    using (var cursor = cmd.ExecuteReader())
                //    {
                //        while (cursor.Read())
                //        {
                //            response = new()
                //            {
                //                //MuyPosibleMuyBajo = (cursor["IN_ID_RIESGO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["IN_ID_RIESGO"]),
                //                //MuyPosibleBajo = (cursor["VC_NOMBRE_RIESGO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["VC_NOMBRE_RIESGO"]),
                //                //MuyPosibleMedio = (cursor["DO_PROBABILIDAD"] == DBNull.Value) ? null : Convert.ToInt32(cursor["DO_PROBABILIDAD"]),
                //                //MuyPosibleAlto = (cursor["DO_IMPACTO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["DO_IMPACTO"]),
                //                //MuyPosibleCritico = (cursor["VC_PRIODIDAD"] == DBNull.Value) ? null : Convert.ToInt32(cursor["VC_PRIODIDAD"]),

                //                //PosibleMuyBajo = (cursor["IN_ID_RIESGO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["IN_ID_RIESGO"]),
                //                //PosibleBajo = (cursor["VC_NOMBRE_RIESGO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["VC_NOMBRE_RIESGO"]),
                //                //PosibleMedio = (cursor["DO_PROBABILIDAD"] == DBNull.Value) ? null : Convert.ToInt32(cursor["DO_PROBABILIDAD"]),
                //                //MuyPosibleAlto = (cursor["DO_IMPACTO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["DO_IMPACTO"]),
                //                //MuyPosibleCritico = (cursor["VC_PRIODIDAD"] == DBNull.Value) ? null : Convert.ToInt32(cursor["VC_PRIODIDAD"]),
                //            };
                //        }
                //    }
                //    cn.Close();

                //    return response;
                //}
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
