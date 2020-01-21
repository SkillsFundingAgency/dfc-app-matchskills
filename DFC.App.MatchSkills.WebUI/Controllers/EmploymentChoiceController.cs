using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.WebUI.Controllers
{
    
    public class EmploymentChoiceController : BaseController
    {

        [HttpGet]
        [Route("/body/EmploymentChoice")]
        public override IActionResult Body()
        {
            return View(ReturnPath("body", "EmploymentChoice"));
        }
    }
}