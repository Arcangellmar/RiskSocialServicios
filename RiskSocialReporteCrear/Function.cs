using Amazon.Lambda.Core;
using RiskSocialReporteCrear.DAO;
using RiskSocialReporteCrear.Domain;
using RiskSocialReporteCrear.Interfaces;
using RiskSocialReporteCrear.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RiskSocialReporteCrear;

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

            databasePort = new DatabaseAdapter(connectionString);

            var IdRiesgo = databasePort.ReporteCrear(input);

            if (IdRiesgo != null)
            {
                responseFunction.Success = true;
                responseFunction.Message = "Reporte creado correctamente";
            }
            else
            {
                responseFunction.Success = false;
                responseFunction.Message = "Sucedio un error al guardar el reporte";
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
