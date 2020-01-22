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
        private const string PathName = "EmploymentChoice";

        [HttpGet]
        [Route("/head/EmploymentChoice")]
        public override IActionResult Head()
        {
            return View(ReturnPath("Head", PathName));
        }

        [HttpGet]
        [Route("/breadcrumb/EmploymentChoice")]
        public override IActionResult Breadcrumb()
        {
            return View(ReturnPath("Breadcrumb", PathName));
        }

        [HttpGet]
        [Route("/bodytop/EmploymentChoice")]
        public override IActionResult BodyTop()
        {
            return View(ReturnPath("bodytop"));
        }

        [HttpGet]
        [Route("/body/EmploymentChoice")]
        public override IActionResult Body()
        {
            return View(ReturnPath("body", PathName));
        }

        [HttpGet]
        [Route("/sidebarright/EmploymentChoice")]
        public override IActionResult SidebarRight()
        {
            return View(ReturnPath("sidebarright"));
        }
    }
}