﻿using System.Threading.Tasks;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Application.ServiceTaxonomy
{
    public interface IServiceTaxonomySearcher
    {
        Task<Skill[]> SearchSkills<TSkills>(string apiPath, string ocpApimSubscriptionKey,string skill);
        Task<Occupation[]> SearchOccupations<TOccupations>(string apiPath, string ocpApimSubscriptionKey,string occupation);
    }
}
