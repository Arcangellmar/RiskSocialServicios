namespace RiskSocialAccionCrear.Domain
{
    public class Request
    {
        public string? NombreAccion { get; set; }
        public string? DescripcionAccion { get; set; }
        public int? IdUsuarioCreador { get; set; }
        public int? IdUsuarioAsignado { get; set; }
        public int? IdProyecto { get; set; }
        public string? Estado { get; set; }
    }
}
