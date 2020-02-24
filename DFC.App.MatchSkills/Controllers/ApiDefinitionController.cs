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
            var apiDefinition = _fileService.ReadAllText(@"Docs\OccupationSearchAuto.json");
            if (string.IsNullOrEmpty(apiDefinition))
                return NoContent();

            return Ok(apiDefinition);
        }
    }
}