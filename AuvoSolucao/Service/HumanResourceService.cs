using AuvoSolucao.Models;
using AuvoSolucao.Repository;
using CloudinaryDotNet;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.Options;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace AuvoSolucao.Service
{
    public class HumanResourceService
    {
        public async Task<string> Processamento(UploadFileViewModel model)
        {
            var pathFolder = await UploadFolder(model);
            await CarregarPasta(pathFolder);
            return "OK";
        }
        private async Task<string> UploadFolder(UploadFileViewModel model)
        {
            string folderName = $"folder_{Guid.NewGuid().ToString()}";
            string appDir = Directory.GetCurrentDirectory();
            string targetPath = Path.Combine(appDir, "FilesTemp");
            string destFile = Path.Combine(targetPath, folderName);

            try
            {
                if (!Directory.Exists(destFile))
                    Directory.CreateDirectory(destFile);

                foreach (var file in model.files)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    RepositoryTemp.nomeArquivo = file.FileName.Split("/")[0];

                    var fullPath = Path.Combine(destFile, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return destFile;
        }



        private async Task<int> CarregarPasta(string pathFolder)
        {
            DirectoryInfo Dir = new DirectoryInfo(pathFolder);
            // Busca automaticamente todos os arquivos em todos os subdiretórios
            FileInfo[] Files = Dir.GetFiles("*", SearchOption.AllDirectories);
            RepositoryTemp.departamentoViewModels = new List<DepartamentoViewModel>();
            RepositoryTemp.QuantidadeArquivo = Files.Count();
            Parallel.ForEach(Files, File => CarregarDados(File));

            return 0;
        }

        private async void CarregarDados(FileInfo file)
        {
            //Random randNum = new Random();
            //await Task.Delay(randNum.Next(10000, 30000));

            DepartamentoViewModel departamento = new DepartamentoViewModel();
            string fileName = file.Name;
            var fileNameArray = fileName.Split(".")[0].Split("-");
            departamento.Departamento = fileNameArray[0];
            departamento.MesVigencia = fileNameArray[1];
            departamento.AnoVigencia = int.Parse(fileNameArray[2]);

            List<FuncionariosCSVModel> funcionarios = new List<FuncionariosCSVModel>();

            //StreamReader reader = new StreamReader(file.FullName, Encoding.GetEncoding(c.TextInfo.ANSICodePage));

            StreamReader reader = new StreamReader(file.FullName, Encoding.Latin1, true);

            var linha = reader.ReadLine();
            while (true)
            {
                linha = reader.ReadLine();
                if (linha == null) break;
                funcionarios.Add(new FuncionariosCSVModel(linha));
            }

            var funcionariosViewModel = await AnalisarDados(funcionarios);

            departamento.Funcionarios = funcionariosViewModel;

            RepositoryTemp.departamentoViewModels.Add(departamento);
        }

        private async Task<List<FuncionariosViewModel>> AnalisarDados(List<FuncionariosCSVModel> funcionarios)
        {
            
            DateTime DataInicial = DateTime.Parse(funcionarios[0].Data);
            DataInicial.AddDays(1 - DataInicial.Day);
            int diasUteis = 0;
            int Mes = DataInicial.Month;

            while (DataInicial.Month == Mes)
            {
                if (DataInicial.DayOfWeek.ToString() != "Saturday" && DataInicial.DayOfWeek.ToString() != "Sunday")
                    diasUteis++;
                DataInicial = DataInicial.AddDays(1);
            }

            var funcionarioAgrupado = funcionarios.GroupBy(f => f.Codigo);
            List<FuncionariosViewModel> funcionariosViewModel = new List<FuncionariosViewModel>();

            foreach (var funcionario in funcionarioAgrupado)
            {
                var elmentInicial = funcionario.ElementAt(0);

                if (string.IsNullOrEmpty(elmentInicial.Nome))
                    continue;

                var ValorHoraStr = elmentInicial.ValorHora.Replace("R$", "").Replace(" ", "");
                double ValorHora = Double.Parse(ValorHoraStr);

                var funcionarioVM = new FuncionariosViewModel();
                funcionarioVM.Nome = elmentInicial.Nome;
                funcionarioVM.Codigo = int.Parse(elmentInicial.Codigo);
                funcionarioVM.ValorHora = ValorHora;

                foreach (var LinhaCSV in funcionario)
                {
                    
                    DateTime data = DateTime.Parse(LinhaCSV.Data);
                    string DiaSemana = data.DayOfWeek.ToString();

                    funcionarioVM.DiasTrabalhados++;

                    TimeSpan Entrada = TimeSpan.Parse(LinhaCSV.Entrada);
                    TimeSpan Saida = TimeSpan.Parse(LinhaCSV.Saida);

                    var almoco = LinhaCSV.Almoco.Replace(" ", "").Split("-");

                    TimeSpan EntradaAlmoco = TimeSpan.Parse(almoco[0]);
                    TimeSpan SaidaAlmoco = TimeSpan.Parse(almoco[1]);

                    int horasTrabalhadas = Saida.Hours - Entrada.Hours - (SaidaAlmoco.Hours - EntradaAlmoco.Hours);

                    funcionarioVM.AdicionarValorReceber(horasTrabalhadas * ValorHora);

                    if (DiaSemana == "Saturday" || DiaSemana == "Sunday")
                    {
                        funcionarioVM.DiasExtras++;
                        funcionarioVM.AdicionarValorHorasExtras(horasTrabalhadas);
                    }
                    else
                    {
                        if(horasTrabalhadas > 8)
                            funcionarioVM.AdicionarValorHorasExtras(horasTrabalhadas - 8);
                        else if (horasTrabalhadas < 8)
                            funcionarioVM.AdicionarValorHorasDebito(8 - horasTrabalhadas);
                    }
                }
                var diasFalta = diasUteis - (funcionarioVM.DiasTrabalhados - funcionarioVM.DiasExtras);

                if(diasFalta > 0)
                {
                    funcionarioVM.DiasFalta = diasFalta;
                    funcionarioVM.AdicionarValorHorasDebito(diasFalta * 8);
                }
                funcionariosViewModel.Add(funcionarioVM);
            }

            return funcionariosViewModel.OrderBy(f => f.Codigo).ToList();
        }
    }
}