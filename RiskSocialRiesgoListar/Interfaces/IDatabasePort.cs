using RiskSocialRiesgoListar.Domain;

namespace RiskSocialRiesgoListar.Interfaces
{
    public interface IDatabasePort
    {
        Response RiesgoListar(Request request);
    }
}
