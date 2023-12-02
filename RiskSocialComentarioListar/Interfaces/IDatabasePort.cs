using RiskSocialComentarioListar.Domain;

namespace RiskSocialComentarioListar.Interfaces
{
    public interface IDatabasePort
    {
        Response ComentarioListar(Request request);
    }
}
