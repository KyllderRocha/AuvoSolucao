using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace AuvoSolucao.Models
{
    public class FuncionariosCSVModel
    {
        public FuncionariosCSVModel(string linha) {
            var linhaseparada = linha.Split(';');
            Codigo = linhaseparada[0];
            Nome = linhaseparada[1];
            ValorHora = linhaseparada[2];
            Data = linhaseparada[3];
            Entrada = linhaseparada[4];
            Saida = linhaseparada[5];
            Almoco = linhaseparada[6];
        }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string ValorHora { get; set; }
        public string Data { get; set; }
        public string Entrada { get; set; }
        public string Saida { get; set; }
        public string Almoco { get; set; }
    }
}