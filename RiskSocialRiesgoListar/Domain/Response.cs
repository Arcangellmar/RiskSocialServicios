namespace RiskSocialRiesgoListar.Domain
{
    public class Response
    {
        public List<Riesgo>? Riesgos { get; set; }
    }
    public class Riesgo
    {
        public string? NombreRiesgo { get; set; }
        public double? Probabilidad { get; set; }
        public double? Impacto { get; set; }
        public string? Prioridad { get; set; }
        public string? Criticidad { get; set; }
        public string? Estado { get; set; }
        public string? Usuario { get; set; }
    }
}
