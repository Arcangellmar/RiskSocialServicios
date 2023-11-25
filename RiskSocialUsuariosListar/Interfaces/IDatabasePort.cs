using RiskSocialUsuariosListar.Domain;

namespace RiskSocialUsuariosListar.Interfaces
{
    public interface IDatabasePort
    {
        Response UsuarioListar(Request request);
    }
}
