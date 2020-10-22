using System;
using System.Diagnostics.CodeAnalysis;
using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DFC.App.MatchSkills.Controllers
{
    [ExcludeFromCodeCoverage]
    public class OccupationSearchDetailsController : CompositeSessionController<OccupationSearchDetailsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        private readonly ISessionService _sessionService;

        public OccupationSearchDetailsController(IServiceTaxonomySearcher serviceTaxonomy,
            IOptions<ServiceTaxonomySettings> settings,
            IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService)
            : base(compositeSettings,
                sessionService)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
            _sessionService = sessionService;
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            return RedirectTo($"{CompositeViewModel.PageId.OccupationSearch}");
        }


        [SessionRequired]
        [HttpGet, HttpPost]
        [Route("body/OccupationSearchDetails")]
        public async Task<IActionResult> Body(string occupationSearchGovUkInputSearch)
        {
            await TrackPageInUserSession();

            var occupations = await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",
                 _settings.ApiKey, occupationSearchGovUkInputSearch, bool.Parse(_settings.SearchOccupationInAltLabels));

            var allReturnedTerms = occupations.SelectMany(x => x.AlternativeNames).Union(occupations.Select(z => z.Name.ToUpperInvariant())).GroupBy(z => z).Select(y => y.Key);
            var termsWithOccupationIds = allReturnedTerms.Select(x => new SearchTermMatch { Name = x.ToUpperInvariant(), Id = occupations.FirstOrDefault(y => y.AlternativeNames.Select(s => s.ToUpperInvariant()).Contains(x.ToUpperInvariant()) || y.Name.ToUpperInvariant() == x.ToUpperInvariant()).Id });

            var termRankingDictionary = new Dictionary<string, decimal>();

            var allMatchingTerms = termsWithOccupationIds.Where(x => x.Name.ToUpperInvariant().Contains(occupationSearchGovUkInputSearch.ToUpperInvariant()));

            foreach (var term in allMatchingTerms)
            {
                var stringWithTermRemovedLength = term.Name.Replace(occupationSearchGovUkInputSearch.ToUpperInvariant(), "").Length;
                var titleCaseTerm = TitleCaseString(term.Name);

                if (stringWithTermRemovedLength == 0)
                {
                    if (!termRankingDictionary.ContainsKey(titleCaseTerm))
                    {
                        termRankingDictionary.Add(TitleCaseString(titleCaseTerm), 100);
                    }
                }
                else
                {
                    if (!termRankingDictionary.ContainsKey(titleCaseTerm))
                    {
                        var matchedCharacters = term.Name.Length - stringWithTermRemovedLength;
                        decimal percentageMatch = ((decimal)matchedCharacters / (decimal)term.Name.Length) * 100;
                        termRankingDictionary.Add(titleCaseTerm, percentageMatch);
                    }
                }
            }

            var orderedDictionaryTerms = termRankingDictionary.OrderByDescending(x => x.Value).Take(100);

            List<Occupation> occupationsToReturn = new List<Occupation>();

            foreach (var orderedItem in orderedDictionaryTerms)
            {
                var occupationsFromPrefLabel = occupations.FirstOrDefault(x => x.Name.ToUpperInvariant() == orderedItem.Key.ToUpperInvariant());

                if(occupationsFromPrefLabel != null)
                {
                    if (!occupationsToReturn.Any(x => x.Id == occupationsFromPrefLabel.Id))
                    {
                        occupationsToReturn.Add(occupationsFromPrefLabel);
                    }
                }

                var occupationsFromAltLabels = occupations.Where(x => x.AlternativeNames.Select(y => y.ToUpperInvariant()).Contains(orderedItem.Key.ToUpperInvariant()));

                foreach(var occupationFromAlt in occupationsFromAltLabels)
                {
                    if (!occupationsToReturn.Any(x => x.Id == occupationFromAlt.Id))
                    {
                        occupationsToReturn.Add(occupationFromAlt);
                    }
                }  
            }

            ViewModel.Occupations = occupationsToReturn.ToArray();

            return await base.Body();
        }

        public static string TitleCaseString(string s)
        {
            if (s == null) return s;

            string[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                char firstChar = char.ToUpper(words[i][0]);
                string rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return string.Join(" ", words);
        }
    }
}
