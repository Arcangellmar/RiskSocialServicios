using MySql.Data.MySqlClient;
using RiskSocialAccionesListar.Domain;
using RiskSocialAccionesListar.Interfaces;
using System.Data;

namespace RiskSocialAccionesListar.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Response AccionesListar(Request request)
        {
            Response response = new();
            response.Acciones = new List<Acciones>();

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_acciones_listar";

                    cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = request.IdProyecto;

                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            Acciones data = new()
                            {
                                NombreAccion = (cursor["VC_NOMBRE_ACCION"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE_ACCION"]),
                                DescripcionAccion = (cursor["VC_DESCRIPCION"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_DESCRIPCION"]),
                                Estado = (cursor["VC_ESTADO"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_ESTADO"]),
                                NombreUsuarioCreador = (cursor["VC_NOMBRE"] == DBNull.Value) ? null : Convert.ToString(cursor["VC_NOMBRE"]),
                                NombreUsuarioAsignado = (cursor["USUARIO_ASIGNADO"] == DBNull.Value) ? null : Convert.ToString(cursor["USUARIO_ASIGNADO"]),
                            };

                            response.Acciones.Add(data);
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
