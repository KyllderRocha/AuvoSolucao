using AuvoSolucao.Models;
using AuvoSolucao.Repository;
using AuvoSolucao.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuvoSolucao.Controllers
{
    public class HumanResourceController : Controller
    {
        private readonly ILogger<HumanResourceController> _logger;
        private readonly HumanResourceService humanResourceService;

        public HumanResourceController(ILogger<HumanResourceController> logger)
        {
            _logger = logger;
            humanResourceService = new HumanResourceService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFolder(UploadFileViewModel model)
        {
            if(model.files == null)
                return View("Index");
            RepositoryTemp.nomeArquivo = string.Empty;
            RepositoryTemp.QuantidadeArquivo = 0;
            await humanResourceService.Processamento(model);
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Download()
        {
            var departamentoViewModels = RepositoryTemp.departamentoViewModels.OrderBy(d => d.Departamento).ToList();
            var jsonstr = System.Text.Json.JsonSerializer.Serialize(departamentoViewModels);
            byte[] byteArray = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonstr);

            return File(byteArray, "application/force-download", RepositoryTemp.nomeArquivo + ".json");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}