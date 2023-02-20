namespace AuvoSolucao.Models
{
    public class FuncionariosViewModel
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string AnoVigencia { get; set; }
        public double TotalReceber { get; set; }
        public double HorasExtras { get; set; }
        public double HorasDebito { get; set; }
        public int DiasFalta { get; set; }
        public int DiasExtras { get; set; }
        public int DiasTrabalhados { get; set; }
    }
}