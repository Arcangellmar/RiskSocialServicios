namespace RiskSocialCrearRiesgo.Domain
{
    public class Request
    {
        public string? NombreRiesgo {  get; set; }
        public string? DescripcionRiesgo { get; set; }
        public double? Probabilidad {  get; set; }
        public double? Impacto {  get; set; }
        public string? Prioricidad {  get; set; }
        public string? Criticidad {  get; set; }
        public int? UsuarioAsignado {  get; set; }
        public int? IdProyecto {  get; set; }
    }
}
