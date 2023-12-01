namespace RiskSocialProblemaListar.Domain
{
    public class Response
    {
        public List<Problema>? Problemas { get; set; }
    }
    public class Problema
    {
        public string? NombreProblema { get; set; }
        public string? Descripcion { get; set; }
        public string? Pioridad { get; set; }
        public string? Criticidad { get; set; }
        public string? Estado { get; set; }
        public string? NombreUsuarioAsignado { get; set; }
        public string? NombreRiesgo { get; set; }
    }
}
