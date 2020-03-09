using DFC.App.MatchSkills.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{
    public class ApiDefinitionController : Controller
    {
        private readonly IFileService _fileService;
       
        public ApiDefinitionController(IFileService fileService)
        {
            _fileService = fileService;
       
        }

        [HttpGet]
        [Route("/apiDefinition")]
       
        public IActionResult Index()
        {
            var hostName = Request.Host.HasValue ? Request.Host.Value : string.Empty;
            var apiDefinition = _fileService.ReadAllText(@"Docs\OccupationSearchAuto.json");
            apiDefinition = apiDefinition.Replace("{serverurl}", hostName);

            if (string.IsNullOrEmpty(apiDefinition))
                return NoContent();

            return Ok(apiDefinition);
        }
    }
}