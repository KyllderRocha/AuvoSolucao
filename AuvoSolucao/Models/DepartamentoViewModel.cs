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
                return Funcionarios.Sum(f => f.ValorTotalReceber);
            }
        }
        public double TotalDescontos { get; set; }
        public double TotalExtras { get; set; }
        public List<FuncionariosViewModel> Funcionarios { get; set; }
    }
}