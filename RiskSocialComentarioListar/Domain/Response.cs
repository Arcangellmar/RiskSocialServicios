namespace RiskSocialComentarioListar.Domain
{
    public class Response
    {
        public List<Comentarios>? Comentarios { get; set; }
    }
    public class Comentarios
    {
        public string? NombreUsuario { get; set; }
        public string? Comentario { get; set; }
    }
}
