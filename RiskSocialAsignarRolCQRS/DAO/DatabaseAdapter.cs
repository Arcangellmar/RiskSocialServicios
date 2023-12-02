using MySql.Data.MySqlClient;
using RiskSocialAsignarRolCQRS.Domain;
using RiskSocialAsignarRolCQRS.Interfaces;
using System.Data;

namespace RiskSocialAsignarRolCQRS.DAO
{
    public class DatabaseAdapter : IDatabasePort
    {
        private readonly string connectionString;

        public DatabaseAdapter(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<RolInsertar>? ListarDatos()
        {
            List<RolInsertar> response = new List<RolInsertar>();

            try
            {
                using (MySqlConnection cn = new(connectionString))
                {
                    MySqlCommand cmd = new();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"sp_roles_insertar_listar";

                    using (var cursor = cmd.ExecuteReader())
                    {
                        while (cursor.Read())
                        {
                            RolInsertar data = new()
                            {
                                IdUsuario = (cursor["IN_ID_USUARIO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["IN_ID_USUARIO"]),
                                IdProyecto = (cursor["IN_ID_PROYECTO"] == DBNull.Value) ? null : Convert.ToInt32(cursor["IN_ID_PROYECTO"]),
                                IdRol = (cursor["IN_ID_ROL"] == DBNull.Value) ? null : Convert.ToInt32(cursor["IN_ID_ROL"]),
                            };

                            response.Add(data);
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
        public bool? InsertarDatos(List<RolInsertar>? data, string connection)
        {
            foreach (var dat in data)
            {
                try
                {
                    using (MySqlConnection cn = new(connection))
                    {
                        MySqlCommand cmd = new();
                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = @"sp_rol_cambiar";

                        cmd.Parameters.Add("@PARAM_IN_ID_USUARIO", MySqlDbType.Int32).Value = dat.IdUsuario;
                        cmd.Parameters.Add("@PARAM_IN_ID_PROYECTO", MySqlDbType.Int32).Value = dat.IdProyecto;
                        cmd.Parameters.Add("@PARAM_IN_ID_ROL", MySqlDbType.Int32).Value = dat.IdRol;

                        string? ReturnBD = cmd.ExecuteScalar().ToString();
                        cn.Close();

                        return ReturnBD == "1";

                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return true;
        }
    }
}
