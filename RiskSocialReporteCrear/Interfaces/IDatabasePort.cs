using RiskSocialReporteCrear.Domain;

namespace RiskSocialReporteCrear.Interfaces
{
    public interface IDatabasePort
    {
        int? ReporteCrear(Request request);
    }
}
