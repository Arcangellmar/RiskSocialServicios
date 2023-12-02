namespace RiskSocialAsignarRolCQRS.Domain
{
    public class Response
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
    public class RolInsertar
    {
        public int? IdUsuario { get; set; }
        public int? IdProyecto { get; set; }
        public int? IdRol { get; set; }
    }
}
