using RiskSocialMatriz.Domain;

namespace RiskSocialMatriz.Interfaces
{
    public interface IDatabasePort
    {
        Response? Matriz(Request request);
    }
}
