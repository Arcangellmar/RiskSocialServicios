using RiskSocialCrearRiesgo.Domain;

namespace RiskSocialCrearRiesgo.Interfaces
{
    public interface IDatabasePort
    {
        int? RiesgoCrear(Request request);
    }
}
