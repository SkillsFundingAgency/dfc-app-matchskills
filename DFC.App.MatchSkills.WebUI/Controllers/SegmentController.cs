using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.WebUI.Controllers
{
    public class SegmentController : Controller
    {
        [HttpGet]
        [Route("/head/{**path}")]
        public IActionResult Head(string path)
        {
            var model = new HeadViewModel()
            {
                CssLink = "https://dev-cdn.nationalcareersservice.org.uk/gds_service_toolkit/css/dysac.min.css",
            };
            return View(ReturnPath(path, "Head"), model);
        }

        [HttpGet]
        [Route("/breadcrumb/{**path}")]
        public IActionResult Breadcrumb(string path)
        {
            return View(ReturnPath(path, "Breadcrumb"));
        }

        [HttpGet]
        [Route("/bodytop/{**path}")]
        public IActionResult BodyTop(string path)
        {
            return View(ReturnPath(path, "bodytop"));
        }

        [HttpGet]
        [Route("/sidebarright/{**path}")]
        public IActionResult SidebarRight(string path)
        {
            return View(ReturnPath(path, "sidebarright"));
        }

        [HttpGet]
        [Route("/body/{**path}")]
        public async Task<IActionResult> Body(string path)
        {
            return View(ReturnPath(path, "body"));
        }

        private string ReturnPath(string path, string segmentName)
        {
            return $"/Views/{(string.IsNullOrWhiteSpace(path) ? "index" : path)}/{segmentName}.cshtml";
        }
    }
}