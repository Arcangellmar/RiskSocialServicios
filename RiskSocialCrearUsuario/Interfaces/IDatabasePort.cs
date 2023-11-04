using RiskSocialCrearUsuario.Domain;

namespace RiskSocialCrearUsuario.Interfaces
{
    public interface IDatabasePort
    {
        bool UsuarioCrear(Request request);
    }
}
