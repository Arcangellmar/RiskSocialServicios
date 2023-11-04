namespace RiskSocialProblemaCrear.Domain
{
    public class Request
    {
        public string? NombreProblema { get; set; }
        public string? DescripcionProblema { get; set; }
        public string? Prioricidad { get; set; }
        public string? Criticidad { get; set; }
        public int? UsuarioAsignado { get; set; }
        public int? IdProyecto { get; set; }
    }
}
