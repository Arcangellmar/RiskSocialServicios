using RiskSocialProblemaListar.Domain;

namespace RiskSocialProblemaListar.Interfaces
{
    public interface IDatabasePort
    {
        Response ProblemaListar(Request request);
    }
}
