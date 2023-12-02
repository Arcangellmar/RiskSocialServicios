namespace RiskSocialAccionesListar.Domain
{
    public class Response
    {
        public List<Acciones>? Acciones { get; set; }
    }
    public class Acciones
    {
        public string? NombreAccion { get; set; }
        public string? DescripcionAccion { get; set; }
        public string? NombreUsuarioCreador { get; set; }
        public string? NombreUsuarioAsignado { get; set; }
        public string? Estado { get; set; }
    }
}
