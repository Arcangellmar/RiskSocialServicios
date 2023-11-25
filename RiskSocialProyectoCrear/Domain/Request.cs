namespace RiskSocialProyectoCrear.Domain
{
    public class Request
    {
        public string? NombreProyecto { get; set; }
        public string? DescripcionProyecto { get; set; }
        public int? UsuarioResponsable { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
        public List<ArchivoS3>? ArchivosS3 { get; set; }
    }
    public class ArchivoS3
    {
        public string? FileB64 {  get; set; }
        public string? NombreArchivo {  get; set; }
        public string? ContentType {  get; set; }
    }
}
