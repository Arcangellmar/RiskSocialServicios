namespace RiskSocialMatriz.Domain
{
    public class Response
    {
        public int? MuyPosibleMuyBajo { get; set; }
        public int? MuyPosibleBajo { get; set; }
        public int? MuyPosibleMedio { get; set; }
        public int? MuyPosibleAlto { get; set; }
        public int? MuyPosibleCritico { get; set; }

        public int? PosibleMuyBajo { get; set; }
        public int? PosibleBajo { get; set; }
        public int? PosibleMedio { get; set; }
        public int? PosibleAlto { get; set; }
        public int? PosibleCritico { get; set; }

        public int? OcasionalMuyBajo { get; set; }
        public int? OcasionalBajo { get; set; }
        public int? OcasionalMedio { get; set; }
        public int? OcasionalAlto { get; set; }
        public int? OcasionalCritico { get; set; }

        public int? ProbableMuyBajo { get; set; }
        public int? ProbableBajo { get; set; }
        public int? ProbableMedio { get; set; }
        public int? ProbableAlto { get; set; }
        public int? ProbableCritico { get; set; }

        public int? ImprobableMuyBajo { get; set; }
        public int? ImprobableBajo { get; set; }
        public int? ImprobableMedio { get; set; }
        public int? ImprobableAlto { get; set; }
        public int? ImprobableCritico { get; set; }

        public List<Riesgo>? Riesgos { get; set; }
    }
    public class Riesgo
    {
        public int? IdRiesgo { get; set; }
        public string? NombreRiesgo { get; set; }
        public double? Probabilidad { get; set; }
        public double? Impacto { get; set; }
        public string? Prioridad { get; set; }
        public string? Criticidad { get; set; }
        public string? Estado { get; set; }
        public string? Usuario { get; set; }
    }
}
