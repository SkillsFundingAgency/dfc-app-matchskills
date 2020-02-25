using System;

namespace DFC.App.MatchSkills.Application.Session.Models
{
    public class UsSkill
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime DateAdded { get; set; }

        public UsSkill(string id, string name)
        {
            Id = id;
            Name = name;
            DateAdded = DateTime.UtcNow;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (null == obj) return false;
            if (!(obj is UsSkill)) return false;
            return Id.Equals(((UsSkill)obj).Id);
        }
    }
}
