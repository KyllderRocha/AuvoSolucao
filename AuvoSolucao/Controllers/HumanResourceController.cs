using AuvoSolucao.Models;
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
        public async Task<string> UploadFolder(UploadFileViewModel model)
        {
            var file = Request.Form.Files[0];

            await humanResourceService.Processamento(model);
            return "OK";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}