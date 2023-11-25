using MySql.Data.MySqlClient;
using RiskSocialUsuarioListarProyectos.Domain;
using RiskSocialUsuarioListarProyectos.Interfaces;
using System.Data;

namespace RiskSocialUsuarioListarProyectos.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Response UsuarioListarProyecto(Request request)
        {
            Response response = new();
            response.Proyectos = new List<Proyecto>();

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_usuario_listar_proyectos";

                    cmd.Parameters.Add("@PARAM_IN_ID_USUARIO", MySqlDbType.Int32).Value = request.IdUsuario;

                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            Proyecto data = new()
                            {
                                IdProyecto = (cursor["ID_PROYECTO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["ID_PROYECTO"]),
                                NombreProyecto = (cursor["VC_NOMBRE_PROYECTO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE_PROYECTO"]),
                                DescripcionProyecto = (cursor["VC_DESCRIPCION_PROYECTO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_DESCRIPCION_PROYECTO"]),
                                FechaInicio = (cursor["DT_FECHA_INICIO"] == DBNull.Value) ? null : Convert.ToString(cursor["DT_FECHA_INICIO"]),
                                FechaFin = (cursor["DT_FECHA_FIN"] == DBNull.Value) ? null : Convert.ToString(cursor["DT_FECHA_FIN"]),
                                Estado = (cursor["VC_ESTADO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_ESTADO"]),
                            };

                            response.Proyectos.Add(data);
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
