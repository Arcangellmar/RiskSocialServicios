using RiskSocialAsignarRol.Domain;

namespace RiskSocialAsignarRol.Interfaces
{
    public interface IDatabasePort
    {
        bool RolCambiar(Request request);
    }
}
