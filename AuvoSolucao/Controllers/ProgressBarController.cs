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
            return Json(RepositoryTemp.PorcentagemAtual);
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