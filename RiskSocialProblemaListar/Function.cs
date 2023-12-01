using Amazon.Lambda.Core;
using RiskSocialProblemaListar.Domain;
using RiskSocialProblemaListar.Interfaces;
using RiskSocialProblemaListar.DAO;
using RiskSocialProblemaListar.Services;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RiskSocialProblemaListar;

public class Function
{
    private IDatabasePort databasePort;
    private readonly S3Service S3Service = new();

    public Response FunctionHandler(Request input, ILambdaContext context)
    {
        LambdaLogger.Log(JsonConvert.SerializeObject(input));
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

            databasePort = new DatabaseAdapter(connectionString);

            var res = databasePort.ProblemaListar(input);

            return res;

        }
        catch (Exception)
        {
        }

        return responseFunction;
    }
}
