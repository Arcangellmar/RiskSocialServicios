namespace RiskSocialUsuariosListar.Domain
{
    public class Response
    {
        public List<Usuario>? Usuarios { get; set; }
    }
    public class Usuario
    {
        public int? IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
    }
}
