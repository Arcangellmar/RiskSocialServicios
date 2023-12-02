using RiskSocialAccionesListar.Domain;

namespace RiskSocialAccionesListar.Interfaces
{
    public interface IDatabasePort
    {
        Response AccionesListar(Request request);
    }
}
