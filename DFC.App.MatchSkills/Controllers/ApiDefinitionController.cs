using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using Dfc.ProviderPortal.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class ApiDefinitionController : Controller
    {
        private readonly IFileService _fileService;
       
        public ApiDefinitionController(IFileService fileService,  IOptions<ServiceTaxonomySettings> settings)
        {
            _fileService = fileService;
       
        }

        [HttpGet]
        [Route("/apiDefinition")]
       
        public IActionResult Index()
        {
            var hostName = Request.Headers["HOSTNAME"].ToString();
            var apiDefinition = _fileService.ReadAllText(@"Docs\OccupationSearchAuto.json");
            apiDefinition = apiDefinition.Replace("{serverurl}", $"{hostName}.azurewebsites.net");

            if (string.IsNullOrEmpty(apiDefinition))
                return NoContent();

            return Ok(apiDefinition);
        }
    }
}