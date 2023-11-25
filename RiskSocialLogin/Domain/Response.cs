namespace RiskSocialLogin.Domain
{
    public class Response
    {
        public int? IdUsuario { get; set; }
        public string? Usuario { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Mensaje { get; set; }
        public bool? Estado { get; set; }
        public string? Pass { get; set; }
    }
}
