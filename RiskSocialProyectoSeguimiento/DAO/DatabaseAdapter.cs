using MySql.Data.MySqlClient;
using Org.BouncyCastle.Math;
using RiskSocialProyectoSeguimiento.Domain;
using RiskSocialProyectoSeguimiento.Interfaces;
using System.Data;

namespace RiskSocialProyectoSeguimiento.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Response ProyectoSeguimiento(Request request)
        {
            Response response = new();
            response.Usuarios = new List<Usuarios>();
            response.Riesgos = new List<Riesgos>();
            response.Reportes = new List<Reportes>();
            response.Problemas = new List<Problemas>();
            response.Acciones = new List<Acciones>();

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_proyecto_seguimiento";

                    cmd.Parameters.Add("@PARAM_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;



                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            Usuarios data = new()
                            {
                                NombreRol = (cursor["VC_NOMBRE_ROL"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE_ROL"]),
                                Usuario = (cursor["VC_USUARIO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_USUARIO"]),
                            };

                            response.Usuarios.Add(data);

                        }
                        if (cursor.NextResult())
                        {
                            while (cursor.Read())
                            {
                                Riesgos data = new()
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
                        if (cursor.NextResult())
                        {
                            while (cursor.Read())
                            {
                                Reportes data = new()
                                {
                                    ContenidoReporte = (cursor["VC_CONTENIDO_REPORTE"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_CONTENIDO_REPORTE"]),
                                    Usuario = (cursor["VC_USUARIO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_USUARIO"]),
                                };

                                response.Reportes.Add(data);
                            }
                        }
                        //if (cursor.NextResult())
                        //{
                        //    while (cursor.Read())
                        //    {
                        //        Problemas data = new()
                        //        {
                        //            NombreProblema = (cursor["VC_NOMBRE_PROBLEMA"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE_PROBLEMA"]),
                        //            Prioridad = (cursor["VC_PRIORIDAD"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_PRIORIDAD"]),
                        //            Criticidad = (cursor["VC_CRITICIDAD"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_CRITICIDAD"]),
                        //            Estado = (cursor["VC_ESTADO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_ESTADO"]),
                        //            Usuario = (cursor["VC_USUARIO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_USUARIO"]),
                        //        };

                        //        response.Problemas.Add(data);
                        //    }
                        //}
                        if (cursor.NextResult())
                        {
                            while (cursor.Read())
                            {
                                Acciones data = new()
                                {
                                    Usuario = (cursor["VC_USUARIO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_USUARIO"]),
                                    NombreAccion = (cursor["VC_NOMBRE_ACCION"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE_ACCION"]),
                                    Descripcion = (cursor["VC_DESCRIPCION"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_DESCRIPCION"]),
                                };

                                response.Acciones.Add(data);
                            }
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
