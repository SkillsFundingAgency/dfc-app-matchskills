using System;

namespace DFC.App.MatchSkills.Application.Session.Models
{
    public class UsOccupation
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public DateTime DateAdded { get; set; }
        

        public UsOccupation(string id, string name)
        {
            Id = id;
            Name = name;
            DateAdded = DateTime.UtcNow;
            
        }
    }
}
