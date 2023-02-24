namespace AuvoSolucao.Models
{
    public class FuncionariosViewModel
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }
        internal double ValorTotalReceber { get; set; }
        public double TotalReceber { 
            get { 
                return Math.Round(ValorTotalReceber, 1);
            } 
        }

        private double ValorHorasExtras { get; set; }
        public double HorasExtras
        {
            get
            {
                return Math.Round(ValorHorasExtras, 2);
            }
        }
        private double ValorHorasDebito { get; set; }
        public double HorasDebito
        {
            get
            {
                return Math.Round(ValorHorasDebito, 2);
            }
        }
        public int DiasFalta { get; set; }
        public int DiasExtras { get; set; }
        public int DiasTrabalhados { get; set; }

        public void AdicionarValorReceber(double valor)
        {
            ValorTotalReceber += valor;
        }

        public void AdicionarValorHorasExtras(double valor)
        {
            ValorHorasExtras += valor;
        }

        public void AdicionarValorHorasDebito(double valor)
        {
            ValorHorasDebito += valor;
        }
    }
}