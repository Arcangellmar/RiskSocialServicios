namespace RiskSocialUsuarioListarProyectos.Domain
{
    public class Response
    {
        public List<Proyecto>? Proyectos { get; set; }
    }
    public class Proyecto
    {
        public int? IdProyecto { get; set; }
        public string? NombreProyecto { get; set; }
        public string? DescripcionProyecto { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
        public string? Estado { get; set; }
    }
}
