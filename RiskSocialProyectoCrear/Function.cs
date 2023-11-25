using Amazon.Lambda.Core;
using Amazon.S3.Model;
using RiskSocialProyectoCrear.DAO;
using RiskSocialProyectoCrear.Domain;
using RiskSocialProyectoCrear.Interfaces;
using RiskSocialProyectoCrear.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RiskSocialProyectoCrear;

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

            var IdProyecto = databasePort.ProyectoCrear(input);

            if (IdProyecto != null)
            {
                var Data = databasePort.ProyectoUsuarioCrear(input.UsuarioResponsable, IdProyecto, 1);

                if (Data)
                {

                    if (input.ArchivosS3 != null)
                    {
                        foreach(var archivo in input.ArchivosS3)
                        {
                            try
                            {
                                if (archivo.FileB64 == null)
                                {
                                    continue;
                                }

                                string key = $"Acciones/{IdProyecto}/{archivo.NombreArchivo}";

                                byte[] dataB64 = Convert.FromBase64String(archivo.FileB64);

                                PutObjectRequest uploadRequest = new()
                                {
                                    BucketName = "risk-social",
                                    Key = key,
                                    InputStream = new MemoryStream(dataB64),
                                    ContentType = archivo.ContentType
                                };

                            }
                            catch (Exception)
                            {

                            }
                        }
                    }

                    responseFunction.Success = true;
                    responseFunction.Message = "Proyecto creado correctamente";
                }
                else
                {
                    responseFunction.Success = false;
                    responseFunction.Message = "Sucedio un error al guardar el proyecto";
                }
            }
            else
            {
                responseFunction.Success = false;
                responseFunction.Message = "Sucedio un error al guardar el proyecto";
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
