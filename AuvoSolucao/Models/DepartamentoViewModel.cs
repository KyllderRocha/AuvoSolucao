namespace AuvoSolucao.Models
{
    public class DepartamentoViewModel { 
    
        public DepartamentoViewModel()
        {
            Funcionarios = new List<FuncionariosViewModel>();
        }

        public string Departamento { get; set; }
        public string MesVigencia { get; set; }
        public int AnoVigencia { get; set; }
        public double TotalPagar { 
            get {
                return Math.Round(Funcionarios.Sum(f => f.ValorTotalReceber),1);
            }
        }

        public double TotalDescontos
        {
            get
            {
                return Math.Round(Funcionarios.Sum(f => f.ValorHora * f.ValorHorasDebito), 1);
            }
        }

        public double TotalExtras
        {
            get
            {
                return Math.Round(Funcionarios.Sum(f => f.ValorHora * f.ValorHorasExtras), 1);
            }
        }

        public List<FuncionariosViewModel> Funcionarios { get; set; }
    }
}