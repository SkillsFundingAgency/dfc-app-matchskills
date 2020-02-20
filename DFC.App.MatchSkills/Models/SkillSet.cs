using System;
using System.Collections.Generic;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Models
{
    [Serializable]
    public class SkillSet : HashSet<Skill>
    {

        public void LoadSkillsToRemove(UserSession userSession)
        {
            this.Clear();
            if (null != userSession)
            {
                foreach (var skill in userSession.SkillsToRemove)
                {
                    this.Add(new Skill(skill.Id, skill.Name));
                }
            }
        }

        public void LoadFrom(UserSession userSession)
        {
            this.Clear();
            if (null != userSession)
            {
                foreach (var skill in userSession.Skills)
                {
                    this.Add(new Skill(skill.Id, skill.Name));
                }
            }
        }

        public void LoadFrom(SkillSet skills)
        {
            this.Clear();
            if (null != skills)
            {
                foreach (var skill in skills)
                {
                    this.Add(new Skill(skill.Id, skill.Name));
                }
            }
        }

        public void LoadFrom(List<Skill> skills)
        {
            this.Clear();
            if (null != skills)
            {
                foreach (var skill in skills)
                {
                    this.Add(new Skill(skill.Id, skill.Name));
                }
            }
        }
    }
}
