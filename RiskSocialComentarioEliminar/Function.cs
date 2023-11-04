using Amazon.Lambda.Core;
using RiskSocialComentarioEliminar.DAO;
using RiskSocialComentarioEliminar.Domain;
using RiskSocialComentarioEliminar.Interfaces;
using RiskSocialComentarioEliminar.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RiskSocialComentarioEliminar;

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

            var SuccessEliminar = databasePort.ComentarioEliminar(input);

            if (SuccessEliminar)
            {
                responseFunction.Success = true;
                responseFunction.Message = "Comentario eliminado correctamente";
            }
            else
            {
                responseFunction.Success = false;
                responseFunction.Message = "Sucedio un error al eliminar el comentario";
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
