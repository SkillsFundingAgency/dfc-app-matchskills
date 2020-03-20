using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Dfc.ProviderPortal.Packages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Services.Dysac
{

    public class DysacService : IDysacSessionReader, IDysacSessionWriter
    {
        
        private readonly IOptions<DysacSettings> _dysacSettings;
        private readonly IRestClient _client;
        private const string ResultsEndpoint = "/result/";
        public DysacService(ILogger<DysacService> log, IRestClient client, IOptions<DysacSettings> dysacSettings)
        {
            Throw.IfNull(dysacSettings, nameof(dysacSettings));
            _client = client;
            _dysacSettings = dysacSettings;
        }

        public Task<DysacServiceResponse> InitiateDysac(string sessionId = "")
        {
            var serviceUrl = _dysacSettings.Value.ApiUrl;
            var response = _client.GetAsync<Task<int>>(serviceUrl);
            
            /* Handle response here and modify Dysac Service Response Accordingly. Only returning test responses for now so we can 
               test both OK and error conditions*/
            return String.IsNullOrEmpty(sessionId)
                ? Task.FromResult(new DysacServiceResponse() {ResponseCode = DysacReturnCode.Ok})
                : Task.FromResult(new DysacServiceResponse() {ResponseCode = DysacReturnCode.Error});  
            
        }

        public DysacJobCategory[] GetDysacJobCategories(string sessionId)
        {
            if (sessionId.IsNullOrEmpty())
            {
                return null;
            }
            var serviceUrl = $"{_dysacSettings.Value.ApiUrl}{ResultsEndpoint}/{sessionId}/short";
            return Mapping.Mapper.Map<DysacJobCategory[]>(TestDysacResults().JobCategories);
            //TODO: Uncomment when DYSAC integration allows
            //var response = await _client.GetAsync<DysacResults>(serviceUrl);
            //if (response != null && response.JobCategories.Any())
            //{
            //    return Mapping.Mapper.Map<DysacJobCategory[]>(response.JobCategories);
            //}
            //return null;



        }

        //TODO: Remove when DYSAC Integration allows
        public static DysacResults TestDysacResults()
        {
            return JsonConvert.DeserializeObject<DysacResults>(
                "{  \"SessionId\": \"2k97xk5kyx63zy\",  \"JobCategories\": [{    \"jobFamilyCode\": \"CAM\",    \"jobFamilyName\": \"Creative and media\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"creative-and-media\",    \"traitsTotal\": 14,    \"total\": 14.0,    \"normalizedTotal\": 4.66666666666667,    \"TraitValues\": [{      \"traitCode\": \"CREATOR\",      \"total\": 5,      \"normalizedTotal\": 1.66666666666667    }, {      \"traitCode\": \"DOER\",      \"total\": 4,      \"normalizedTotal\": 1.33333333333333    }, {      \"traitCode\": \"ANALYST\",      \"total\": 5,      \"normalizedTotal\": 1.66666666666667    }],    \"filterAssessment\": null,    \"totalQuestions\": 5,    \"resultsShown\": false  }, {    \"jobFamilyCode\": \"CAT\",    \"jobFamilyName\": \"Construction and trades\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"construction-and-trades\",    \"traitsTotal\": 14,    \"total\": 14.0,    \"normalizedTotal\": 4.66666666666667,    \"TraitValues\": [{      \"traitCode\": \"CREATOR\",      \"total\": 5,      \"normalizedTotal\": 1.66666666666667    }, {      \"traitCode\": \"DOER\",      \"total\": 4,      \"normalizedTotal\": 1.33333333333333    }, {      \"traitCode\": \"ANALYST\",      \"total\": 5,      \"normalizedTotal\": 1.66666666666667    }],    \"filterAssessment\": null,    \"totalQuestions\": 4,    \"resultsShown\": false  }, {    \"jobFamilyCode\": \"SAL\",    \"jobFamilyName\": \"Sports and leisure\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"sports-and-leisure\",    \"traitsTotal\": 11,    \"total\": 11.0,    \"normalizedTotal\": 3.66666666666667,    \"TraitValues\": [{      \"traitCode\": \"CREATOR\",      \"total\": 5,      \"normalizedTotal\": 1.66666666666667    }, {      \"traitCode\": \"ANALYST\",      \"total\": 5,      \"normalizedTotal\": 1.66666666666667    }, {      \"traitCode\": \"DRIVER\",      \"total\": 1,      \"normalizedTotal\": 0.333333333333333    }],    \"filterAssessment\": null,    \"totalQuestions\": 3,    \"resultsShown\": false  }, {    \"jobFamilyCode\": \"CTAD\",    \"jobFamilyName\": \"Computing, technology and digital\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"computing-technology-and-digital\",    \"traitsTotal\": 10,    \"total\": 10.0,    \"normalizedTotal\": 5.0,    \"TraitValues\": [{      \"traitCode\": \"CREATOR\",      \"total\": 5,      \"normalizedTotal\": 2.5    }, {      \"traitCode\": \"ANALYST\",      \"total\": 5,      \"normalizedTotal\": 2.5    }],    \"filterAssessment\": null,    \"totalQuestions\": 3,    \"resultsShown\": false  }, {    \"jobFamilyCode\": \"AC\",    \"jobFamilyName\": \"Animal care\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"animal-care\",    \"traitsTotal\": 6,    \"total\": 6.0,    \"normalizedTotal\": 3.0,    \"TraitValues\": [{      \"traitCode\": \"HELPER\",      \"total\": 2,      \"normalizedTotal\": 1.0    }, {      \"traitCode\": \"DOER\",      \"total\": 4,      \"normalizedTotal\": 2.0    }],    \"filterAssessment\": null,    \"totalQuestions\": 3,    \"resultsShown\": false  }, {    \"jobFamilyCode\": \"HEALT\",    \"jobFamilyName\": \"Healthcare\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"healthcare\",    \"traitsTotal\": 6,    \"total\": 6.0,    \"normalizedTotal\": 3.0,    \"TraitValues\": [{      \"traitCode\": \"HELPER\",      \"total\": 2,      \"normalizedTotal\": 1.0    }, {      \"traitCode\": \"DOER\",      \"total\": 4,      \"normalizedTotal\": 2.0    }],    \"filterAssessment\": null,    \"totalQuestions\": 4,    \"resultsShown\": false  }, {    \"jobFamilyCode\": \"BAW\",    \"jobFamilyName\": \"Beauty and wellbeing\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"beauty-and-wellbeing\",    \"traitsTotal\": 5,    \"total\": 5.0,    \"normalizedTotal\": 2.5,    \"TraitValues\": [{      \"traitCode\": \"DOER\",      \"total\": 4,      \"normalizedTotal\": 2.0    }, {      \"traitCode\": \"DRIVER\",      \"total\": 1,      \"normalizedTotal\": 0.5    }],    \"filterAssessment\": null,    \"totalQuestions\": 2,    \"resultsShown\": false  }, {    \"jobFamilyCode\": \"EAM\",    \"jobFamilyName\": \"Engineering and maintenance\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"engineering-and-maintenance\",    \"traitsTotal\": 4,    \"total\": 4.0,    \"normalizedTotal\": 4.0,    \"TraitValues\": [{      \"traitCode\": \"DOER\",      \"total\": 4,      \"normalizedTotal\": 4.0    }],    \"filterAssessment\": null,    \"totalQuestions\": 5,    \"resultsShown\": false  }, {    \"jobFamilyCode\": \"EAL\",    \"jobFamilyName\": \"Environment and land\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"environment-and-land\",    \"traitsTotal\": 4,    \"total\": 4.0,    \"normalizedTotal\": 4.0,    \"TraitValues\": [{      \"traitCode\": \"DOER\",      \"total\": 4,      \"normalizedTotal\": 4.0    }],    \"filterAssessment\": null,    \"totalQuestions\": 4,    \"resultsShown\": false  }, {    \"jobFamilyCode\": \"EAMA\",    \"jobFamilyName\": \"Engineering and maintenance Amended\",    \"jobFamilyText\": \"\",    \"jobFamilyUrl\": \"engineering-and-maintenance-amended\",    \"traitsTotal\": 4,    \"total\": 4.0,    \"normalizedTotal\": 4.0,    \"TraitValues\": [{      \"traitCode\": \"DOER\",      \"total\": 4,      \"normalizedTotal\": 4.0    }],    \"filterAssessment\": null,    \"totalQuestions\": 5,    \"resultsShown\": false  }],  \"Traits\": [\"you like dealing with complicated problems or working with numbers\", \"you are a creative person and enjoy coming up with new ways of doing things\", \"you are a practical person and enjoy getting things done\"],  \"JobFamilyCount\": 10,  \"JobFamilyMoreCount\": 7,  \"AssessmentType\": \"short\",  \"JobProfiles\": [],  \"WhatYouToldUs\": []}");
        }
    }

   
}
