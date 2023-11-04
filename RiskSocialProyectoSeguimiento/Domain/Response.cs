namespace RiskSocialProyectoSeguimiento.Domain
{
    public class Response
    {
        public List<Usuarios>? Usuarios { get; set; }
        public List<Riesgos>? Riesgos { get; set; }
        public List<Reportes>? Reportes { get; set; }
        public List<Problemas>? Problemas { get; set; }
        public List<Acciones>? Acciones { get; set; }
    }
    public class Usuarios
    {
        public string? NombreRol { get; set; }
        public string? Usuario { get; set; }
    }
    public class Riesgos
    {
        public string? NombreRiesgo { get; set; }
        public double? Probabilidad { get; set; }
        public double? Impacto { get; set; }
        public string? Prioridad { get; set; }
        public string? Criticidad { get; set; }
        public string? Estado { get; set; }
        public string? Usuario { get; set; }
    }
    public class Reportes
    {
        public string? ContenidoReporte { get; set; }
        public string? Usuario { get; set; }
    }
    public class Problemas
    {
        public string? NombreProblema { get; set; }
        public string? Prioridad { get; set; }
        public string? Criticidad { get; set; }
        public string? Estado { get; set; }
        public string? Usuario { get; set; }
    }
    public class Acciones
    {
        public string? NombreAccion { get; set; }
        public string? Descripcion { get; set; }
        public string? Usuario { get; set; }
    }
}
