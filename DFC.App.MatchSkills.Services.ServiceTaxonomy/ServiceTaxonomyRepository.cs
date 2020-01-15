using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;


namespace DFC.App.MatchSkills.Services.ServiceTaxonomy
{
    public class StSkills
    {
        public Skill[] Skills;
    }
    public class StOccupations
    {
        public Occupation[] Occupations;
    }
    public class ServiceTaxonomyRepository : IServiceTaxonomyReader, IServiceTaxonomySearcher
    {
        private static RestClient _restClient;
        
        public ServiceTaxonomyRepository()
        {
            _restClient = new RestClient();
        }
        
        public ServiceTaxonomyRepository(RestClient restClient)
        { 
            _restClient = restClient??new RestClient();
        }
        
        static async Task<TList> GetJsonListGet<TList>(string apiPath, string ocpApimSubscriptionKey) where TList : class
        { 
            if (string.IsNullOrWhiteSpace(apiPath))
                throw new ArgumentNullException(nameof(apiPath), "ApiPath must be specified.");

            if (string.IsNullOrWhiteSpace(ocpApimSubscriptionKey))
                throw new ArgumentNullException(nameof(ocpApimSubscriptionKey),
                    "Ocp-Apim-Subscription-Key must be specified.");

            return await _restClient.GetAsync<TList>(apiPath,ocpApimSubscriptionKey);
            
        }
       
        static async Task<TList> GetJsonListPost<TList>(string apiPath, string ocpApimSubscriptionKey, HttpContent postData) where TList : class
        { 
            if (string.IsNullOrWhiteSpace(apiPath))
                throw new ArgumentNullException(nameof(apiPath), "ApiPath must be specified.");

            if (string.IsNullOrWhiteSpace(ocpApimSubscriptionKey))
                throw new ArgumentNullException(nameof(ocpApimSubscriptionKey),
                    "Ocp-Apim-Subscription-Key must be specified.");
           
            return await _restClient.PostAsync<TList>(apiPath, postData,ocpApimSubscriptionKey);
            
        }
        
        public async Task<Skill[]> GetAllSkills<TSkills>(string apiPath, string ocpApimSubscriptionKey)
        {
            var result = await GetJsonListGet<StSkills>($"{apiPath}/GetAllSkills/Execute/", ocpApimSubscriptionKey); 
            return Mapping.Mapper.Map<Skill[]>(result.Skills);
        }

        public async Task<Occupation[]> GetAllOccupations<TOccupations>(string apiPath, string ocpApimSubscriptionKey) 
        {
            var result = await GetJsonListGet<StOccupations>($"{apiPath}/GetAllOccupations/Execute/" , ocpApimSubscriptionKey);
            return Mapping.Mapper.Map<Occupation[]>(result.Occupations);
        }

        public async Task<Skill[]> SearchSkills<TSkills>(string apiPath, string ocpApimSubscriptionKey, string skill)
        {
            if (string.IsNullOrWhiteSpace((skill)))
                throw new ArgumentNullException(nameof(skill), "Please provide Skill to search");
            var regEx = new System.Text.RegularExpressions.Regex(skill);
            var result = await GetAllSkills<Skill[]>(apiPath, ocpApimSubscriptionKey);
            return result.Where(s => regEx.IsMatch(s.Name)).ToArray();
        }

        public async Task<Occupation[]> SearchOccupations<TOccupations>(string apiPath, string ocpApimSubscriptionKey, string occupation,bool matchAltLabels)
        {
            if (string.IsNullOrWhiteSpace(occupation))
                throw new ArgumentNullException(nameof(occupation), "Please provide Occupation to search");
            
            var postData = new StringContent($"{{ \"label\": \"{occupation.ToLower()}\"}}", Encoding.UTF8, MediaTypeNames.Application.Json);
            var result = await GetJsonListPost<StOccupationSearchResult.OccupationSearchResult>($"{apiPath}GetOccupationsByLabel/Execute/?matchAltLabels={matchAltLabels}", ocpApimSubscriptionKey,postData);
            
            return Mapping.Mapper.Map<Occupation[]>(result.Occupations);
        }
    }
    
}
