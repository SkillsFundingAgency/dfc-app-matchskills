using DFC.App.MatchSkills.Models;
using System.Collections.Generic;

namespace DFC.App.MatchSkills.ViewModels
{
    public class EndPointsViewModel
    {
        public List<EndPoint> EndPoints { get; set; }

        public EndPointsViewModel()
        {
            EndPoints = new List<EndPoint>();
        }
    }
}
