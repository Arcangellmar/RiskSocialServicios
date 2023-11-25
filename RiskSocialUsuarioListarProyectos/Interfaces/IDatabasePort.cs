using RiskSocialUsuarioListarProyectos.Domain;

namespace RiskSocialUsuarioListarProyectos.Interfaces
{
    public interface IDatabasePort
    {
        Response UsuarioListarProyecto(Request request);
    }
}
