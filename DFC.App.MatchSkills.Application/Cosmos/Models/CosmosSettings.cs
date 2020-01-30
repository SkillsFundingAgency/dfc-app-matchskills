using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.MatchSkills.Application.Cosmos.Models
{
    public class CosmosSettings
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string DatabaseName { get; set; }
        public string UserSessionsCollection { get; set; }
    }
}
