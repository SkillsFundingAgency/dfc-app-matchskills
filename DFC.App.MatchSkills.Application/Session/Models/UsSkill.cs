using System;


namespace DFC.App.MatchSkills.Application.Session.Models
{
    public class UsSkill
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime DateAdded { get; set; }

        public UsSkill(string id, string name,DateTime dateAdded)
        {
            Id = id;
            Name = name;
            DateAdded = dateAdded;
        }

    }
}
