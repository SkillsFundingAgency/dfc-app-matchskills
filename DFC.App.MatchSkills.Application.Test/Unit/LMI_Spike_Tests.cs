using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DFC.Personalisation.Common.Net.RestClient;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Application.Test.Unit
{
    
    public class LMI_Spike_Tests
    {
        private const string ApiUrl = "http://api.lmiforall.org.uk/api/v1/";
        private const string SocSearchPath = "soc/search";
        private const string WfPredictSearchPath = "wf/predict";
        private readonly RestClient client = new RestClient();

        [Test]
        public void Call_SOC_Get_Value_Returned()
        {
            var result = client.Get<IEnumerable<SocSearchResults>>($"{ApiUrl}{SocSearchPath}?q=Developer").Result;
            

            var socId = result.Select(x => x.Soc).FirstOrDefault();

            var prediction = client.Get<WfSearchResults>($"{ApiUrl}{WfPredictSearchPath}?soc={socId}").Result;
            Dictionary<string, decimal> growth = new Dictionary<string, decimal>();
            for(var i = 0; i < prediction.PredictedEmployment.Count; i++)
            {
                if (i != prediction.PredictedEmployment.Count - 1)
                {
                    var pastYearValue = prediction.PredictedEmployment[i].Employment;
                    var futureYearValue = prediction.PredictedEmployment[i + 1].Employment;

                    growth.Add($"{prediction.PredictedEmployment[i].Year}-{prediction.PredictedEmployment[i + 1].Year}", GrowthCalc(pastYearValue, futureYearValue));
                }
            }
        }

        public decimal GrowthCalc(long pastYear, long futureYear)
        {
            if (pastYear == 0)
                throw new InvalidOperationException();

            var change = futureYear - pastYear;
            var percentage =  ((decimal)change / pastYear) * 100;
            return Math.Truncate(percentage * 1000m) / 1000m;

        }
    }

    public class LmiApiSettings
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
    }
    public class SocSearchResults
    {
        public int Soc { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Qualifications { get; set; }
        public string Tasks { get; set; }
        public List<string> Add_titles { get; set; }
    }
    public class PredictedEmployment
    {
        public int Year { get; set; }
        public int Employment { get; set; }
    }

    public class WfSearchResults
    {
        public int Soc { get; set; }
        public List<PredictedEmployment> PredictedEmployment { get; set; }
    }
   
}
