using AuvoSolucao.Models;

namespace AuvoSolucao.Repository
{
    public static class RepositoryTemp
    {
        public static List<DepartamentoViewModel> departamentoViewModels { get; set; }
        public static string nomeArquivo { get; set; }
        public static double QuantidadeArquivo { get; set; }
        public static double PorcentagemAtual { 
            get {
                return departamentoViewModels.Count() / QuantidadeArquivo * 100;
            }
        }

    }
}
