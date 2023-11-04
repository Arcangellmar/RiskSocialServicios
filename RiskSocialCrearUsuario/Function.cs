using Amazon.Lambda.Core;
using RiskSocialCrearUsuario.DAO;
using RiskSocialCrearUsuario.Domain;
using RiskSocialCrearUsuario.Interfaces;
using RiskSocialCrearUsuario.Services;
using RiskSocialCrearUsuario.Utils;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RiskSocialCrearUsuario;

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
            if (string.IsNullOrEmpty(input.Pass))
            {
                throw new Exception("Contraña no puede ser vacia");
            }

            // Obtengo config
            var config = S3Service.GetConfig();

            if (config.ConnectionString == null)
            {
                throw new Exception("Error al obtener archivo de configuracion");
            }

            string connectionString = config.ConnectionString;

            databasePort = new DatabaseAdapter(connectionString);


            // Encriptacion de contraseña
            input.Pass = Encry.HashPassword(input.Pass);

            var data = databasePort.UsuarioCrear(input);

            if (data)
            {
                responseFunction.Success = true;
                responseFunction.Message = "Usuario creado correctamente";
            }
            else
            {
                responseFunction.Success = false;
                responseFunction.Message = "Sucedio un error al guardar el usuario";
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
