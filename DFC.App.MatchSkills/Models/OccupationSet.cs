using DFC.App.MatchSkills.Application.Session.Models;
using DFC.Personalisation.Domain.Models;
using System.Collections.Generic;

namespace DFC.App.MatchSkills.Models
{
    public class OccupationSet : HashSet<Occupation>
    {
        public void LoadFromSession(UserSession userSession)
        {
            if (userSession?.Occupations == null || userSession.Occupations.Count == 0)
            {
                Clear();
            }
            else
            {
                foreach (var occupation in userSession.Occupations)
                {
                    Add(new Occupation(occupation.Id, occupation.Name, occupation.DateAdded));
                }
            }
        }
    }
}
