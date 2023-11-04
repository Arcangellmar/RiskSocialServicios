using RiskSocialAccionAsignar.Domain;

namespace RiskSocialAccionAsignar.Interfaces
{
    public interface IDatabasePort
    {
        bool AccionActualizar(Request request);
    }
}
