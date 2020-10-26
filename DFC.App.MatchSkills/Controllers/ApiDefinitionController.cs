using DFC.App.MatchSkills.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

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
            string apiSuffix = Environment.GetEnvironmentVariable("ApiSuffix");

            var apiDefinition = _fileService.ReadAllText(@"Docs\OccupationSearchAuto.json");

            apiDefinition = apiDefinition.Replace("{serverurl}", hostName);
            apiDefinition = apiDefinition.Replace("{apisuffix}", apiSuffix);

            if (string.IsNullOrEmpty(apiDefinition))
                return NoContent();

            return Ok(apiDefinition);
        }
    }
}