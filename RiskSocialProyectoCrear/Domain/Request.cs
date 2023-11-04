namespace RiskSocialProyectoCrear.Domain
{
    public class Request
    {
        public string? NombreProyecto { get; set; }
        public string? DescripcionProyecto { get; set; }
        public int? UsuarioResponsable { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
    }
}
