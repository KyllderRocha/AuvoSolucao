using AuvoSolucao.Models;
using AuvoSolucao.Repository;
using AuvoSolucao.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

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

            var options = new JsonSerializerOptions();
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            var jsonstr = System.Text.Json.JsonSerializer.Serialize(departamentoViewModels, options);
            byte[] byteArray = System.Text.ASCIIEncoding.Default.GetBytes(jsonstr);

            return File(byteArray, "application/force-download", RepositoryTemp.nomeArquivo + ".json");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}