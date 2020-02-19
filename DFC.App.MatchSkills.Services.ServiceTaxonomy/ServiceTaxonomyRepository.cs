using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy
{
    public class StSkills
    {
        public StSkill[] Skills { get; set; }
    }
    public class StOccupations
    {
        public StOccupation[] Occupations { get; set; }
    }
    public class ServiceTaxonomyRepository : IServiceTaxonomyReader, IServiceTaxonomySearcher
    {
        private readonly RestClient _restClient;
        
        public ServiceTaxonomyRepository()
        {
            _restClient = new RestClient();
        }
        
        public ServiceTaxonomyRepository(RestClient restClient)
        { 
            _restClient = restClient??new RestClient();
        }
        
        private async Task<TList> GetJsonListGet<TList>(string apiPath, string ocpApimSubscriptionKey) where TList : class
        { 
            return await _restClient.GetAsync<TList>(apiPath,ocpApimSubscriptionKey);
        }

        private async Task<TList> GetJsonListPost<TList>(string apiPath, string ocpApimSubscriptionKey, HttpContent postData) where TList : class
        {
            return await _restClient.PostAsync<TList>(apiPath, postData,ocpApimSubscriptionKey);
        }
        
        public async Task<Skill[]> GetAllSkills<TSkills>(string apiPath, string ocpApimSubscriptionKey)
        {
            var result = await GetJsonListGet<StSkills>($"{apiPath}/GetAllSkills/Execute/", ocpApimSubscriptionKey); 
            return Mapping.Mapper.Map<Skill[]>(result.Skills);
        }

        public async Task<Skill[]> GetAllSkillsForOccupation<TSkills>(string apiPath, string ocpApimSubscriptionKey, string occupation)
        {
            occupation ??= ""; 
            var postData = new StringContent($"{{ \"uri\": \"{occupation.ToLower()}\"}}", Encoding.UTF8, MediaTypeNames.Application.Json);
            var result = await GetJsonListPost<StOccupationSkills>($"{apiPath}/GetAllSkillsForOccupation/Execute/", ocpApimSubscriptionKey,postData);
            return Mapping.Mapper.Map<Skill[]>(result.Skills);
        }


        public async Task<Occupation[]> GetAllOccupations<TOccupations>(string apiPath, string ocpApimSubscriptionKey) 
        {
            var result = await GetJsonListGet<StOccupations>($"{apiPath}/GetAllOccupations/Execute/" , ocpApimSubscriptionKey);
            return Mapping.Mapper.Map<Occupation[]>(result.Occupations);
        }

        public async Task<Skill[]> SearchSkills<TSkills>(string apiPath, string ocpApimSubscriptionKey, string skill)
        {
            skill ??= "";
            var regEx = new System.Text.RegularExpressions.Regex(skill);
            var result = await GetAllSkills<Skill[]>(apiPath, ocpApimSubscriptionKey);
            return result.Where(s => regEx.IsMatch(s.Name)).ToArray();
        }


        public async Task<Occupation[]> SearchOccupations<TOccupations>(string apiPath, string ocpApimSubscriptionKey, string occupation,bool matchAltLabels)
        {

            occupation ??= ""; 
            var postData = new StringContent($"{{ \"label\": \"{occupation.ToLower()}\"}}", Encoding.UTF8, MediaTypeNames.Application.Json);
            var result = await GetJsonListPost<StOccupationSearchResult.OccupationSearchResult>($"{apiPath}/GetOccupationsByLabel/Execute/?matchAltLabels={matchAltLabels}", ocpApimSubscriptionKey,postData);
            
            return Mapping.Mapper.Map<Occupation[]>(result.Occupations);
        }

        public async Task<OccupationMatch[]> FindOccupationsForSkills(string apiPath, string ocpApimSubscriptionKey, string[] skillIds, int minimumMatchingSkills)
        {
            var request = new GetOccupationsWithMatchingSkillsRequest()
            {
                MinimumMatchingSkills = minimumMatchingSkills,
            };
            foreach (var skill in skillIds)
            {
                request.SkillList.Add(skill);
            }

            var jsonPayload = JsonConvert.SerializeObject(request);
            var postData = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await GetJsonListPost<GetOccupationsWithMatchingSkillsResponse>($"{apiPath}/GetOccupationsWithMatchingSkills/Execute", ocpApimSubscriptionKey, postData);

            var result = Mapping.Mapper.Map<OccupationMatch[]>(response.MatchingOccupations);
            return result;
        }
    }
    
}
