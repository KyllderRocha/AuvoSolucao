using AuvoSolucao.Models;
using AuvoSolucao.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuvoSolucao.Controllers
{
    public class ProgressBarController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ProgressBarController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Progress()
        {
            //var obj = new
            //{
            //    PorcentagemAtual = RepositoryTemp.PorcentagemAtual,
            //    QuantidadeArquivo = RepositoryTemp.QuantidadeArquivo,
            //    nomeArquivo = RepositoryTemp.nomeArquivo,
            //    arquivos = RepositoryTemp.departamentoViewModels.Count()
            //};
            return Json(RepositoryTemp.PorcentagemAtual);
            //return Json(obj);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}