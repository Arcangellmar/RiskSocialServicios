namespace RiskComentarioCrear.Domain
{
    public class Request
    {
        public int? IdUsuarioCreador { get; set; }
        public string? Comentario { get; set; }
        public int? IdReporte { get; set; }
        public int? IdRiesgo { get; set; }
        public int? IdAccion { get; set; }
        public int? IdProblema { get; set; }
    }
}
