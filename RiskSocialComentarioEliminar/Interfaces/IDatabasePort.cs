using RiskSocialComentarioEliminar.Domain;

namespace RiskSocialComentarioEliminar.Interfaces
{
    public interface IDatabasePort
    {
        bool ComentarioEliminar(Request request);
    }
}
