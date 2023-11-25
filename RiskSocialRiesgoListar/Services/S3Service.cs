using Amazon.S3;
using Amazon.S3.Model;
using RiskSocialRiesgoListar.Domain;
namespace RiskSocialRiesgoListar.Services
{
    public class S3Service
    {

        private IAmazonS3 S3Client = new AmazonS3Client();

        public ConfigApp GetConfig()
        {

            string bucketName = "risk-social";
            string key = "Configuracion/conectionStringServicios.json";

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            using (GetObjectResponse response = S3Client.GetObjectAsync(request).Result)
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string jsonString = reader.ReadToEnd();
                ConfigApp? jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigApp>(jsonString);

                if (jsonObject == null)
                {
                    throw new Exception("Error al obtener archivo de configuracion");
                }

                return jsonObject;
            }
        }
    }
}
