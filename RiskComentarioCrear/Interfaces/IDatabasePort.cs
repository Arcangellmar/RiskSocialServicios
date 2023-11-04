using RiskComentarioCrear.Domain;

namespace RiskComentarioCrear.Interfaces
{
    public interface IDatabasePort
    {
        int? ComentarioCrear(Request request);
    }
}
