using RiskSocialProyectoSeguimiento.Domain;

namespace RiskSocialProyectoSeguimiento.Interfaces
{
    public interface IDatabasePort
    {
        Response ProyectoSeguimiento(Request request);
    }
}
