using Amazon.Lambda.Core;
using RiskSocialLogin.DAO;
using RiskSocialLogin.Domain;
using RiskSocialLogin.Interfaces;
using RiskSocialLogin.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RiskSocialLogin;

public class Function
{


    private IDatabasePort databasePort;
    private readonly S3Service S3Service = new();

    public Response? FunctionHandler(Request input, ILambdaContext context)
    {
        Response? responseFunction = new();

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

            databasePort = new DatabaseAdapter(connectionString);

            responseFunction = databasePort.Login(input);

            if (responseFunction != null)
            {
                responseFunction.Mensaje = "Exitoso";
                responseFunction.Estado = true;
            }
            else
            {
                responseFunction.Mensaje = "Sucedio un error al loguearse";
                responseFunction.Estado = false;
            }

        }
        catch (Exception ex)
        {
            responseFunction.Mensaje = ex.Message;
            responseFunction.Estado = false;
        }

        return responseFunction;
    }
}
