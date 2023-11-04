using RiskSocialAccionCrear.Domain;

namespace RiskSocialAccionCrear.Interfaces
{
    public interface IDatabasePort
    {
        int? AccionCrear(Request request);
    }
}
