using Amazon.Lambda.Core;
using RiskSocialAsignarRolCQRS.DAO;
using RiskSocialAsignarRolCQRS.Domain;
using RiskSocialAsignarRolCQRS.Interfaces;
using RiskSocialAsignarRolCQRS.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RiskSocialAsignarRolCQRS;

public class Function
{

    private IDatabasePort databasePort;
    private readonly S3Service S3Service = new();

    public Response FunctionHandler(Request input, ILambdaContext context)
    {
        Response responseFunction = new();

        try
        {
            // Validaciones

            // Obtengo config
            var config = S3Service.GetConfig();

            if (config.ConnectionString == null)
            {
                throw new Exception("Error al obtener archivo de configuracion");
            }

            string connectionString = config.ConnectionString;

            databasePort = new DatabaseAdapter(config.connectionStringEscritura);

            var data = databasePort.ListarDatos();

            if (data != null)
            {
                var success = databasePort.InsertarDatos(data, connectionString);

                if (success ?? false)
                {
                    responseFunction.Success = true;
                    responseFunction.Message = "Roles migrados correctamente";
                }
                else
                {
                    responseFunction.Success = false;
                    responseFunction.Message = "No se pudo cargar la informacion correctamente";
                }
            }
            else
            {
                responseFunction.Success = false;
                responseFunction.Message = "No se pudo obtener la informacion";
            }

        }
        catch (Exception e)
        {
            responseFunction.Success = false;
            responseFunction.Message = e.Message;
        }

        return responseFunction;
    }
}
