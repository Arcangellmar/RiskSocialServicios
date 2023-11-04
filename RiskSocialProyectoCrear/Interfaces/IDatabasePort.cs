using RiskSocialProyectoCrear.Domain;

namespace RiskSocialProyectoCrear.Interfaces
{
    public interface IDatabasePort
    {
        int? ProyectoCrear(Request request);
        bool ProyectoUsuarioCrear(int? IdUsuario, int? IdProyecto, int? IdRol);
    }
}
