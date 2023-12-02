using RiskSocialAsignarRolCQRS.Domain;

namespace RiskSocialAsignarRolCQRS.Interfaces
{
    public interface IDatabasePort
    {
        List<RolInsertar>? ListarDatos();
        bool? InsertarDatos(List<RolInsertar>? data, string connection);
    }
}
