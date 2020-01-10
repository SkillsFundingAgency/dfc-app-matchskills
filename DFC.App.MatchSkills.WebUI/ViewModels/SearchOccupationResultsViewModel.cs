using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.WebUI.ViewModels
{
    public class SearchOccupationResultsViewModel
    {
        public string Title { get; set; }
        public Occupation[] Occupations { get; set; }
    }
}
