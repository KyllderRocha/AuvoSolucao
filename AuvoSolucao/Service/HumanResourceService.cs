using AuvoSolucao.Models;
using AuvoSolucao.Repository;
using CloudinaryDotNet;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
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

            Directory.Delete(pathFolder);
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

            foreach (FileInfo File in Files)
            {
                await CarregarDados(File);
            }
            return 0;
        }

        private async Task<int> CarregarDados(FileInfo file)
        {
            DepartamentoViewModel departamento = new DepartamentoViewModel();
            string fileName = file.Name;
            var fileNameArray = fileName.Split(".")[0].Split("-");
            departamento.Departamento = fileNameArray[0];
            departamento.MesVigencia = fileNameArray[1];
            departamento.AnoVigencia = fileNameArray[2];
            List<FuncionariosCSVModel> funcionarios = new List<FuncionariosCSVModel>();

            StreamReader reader = new StreamReader(file.FullName, Encoding.UTF8, true);
            var linha = reader.ReadLine();
            while (true)
            {
                linha = reader.ReadLine();
                if (linha == null) break;
                funcionarios.Add(new FuncionariosCSVModel(linha));
            }
            return 0;
        }
        private async void AnalisarDados(FuncionariosCSVModel funcionario)
        {
            
        }
    }
}