using RiskSocialProblemaCrear.Domain;

namespace RiskSocialProblemaCrear.Interfaces
{
    public interface IDatabasePort
    {
        int? ProblemaCrear(Request request);
    }
}
