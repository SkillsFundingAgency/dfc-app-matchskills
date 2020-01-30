using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public abstract class CompositeViewModel
    {
        public static string AppTitle => "Discover your skills and careers";

        public class PageId
        {
            private PageId(string value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value;
            }

            public string Value { get; }

            public static PageId Home { get; } = new PageId("home");
            public static PageId Worked { get; } = new PageId("worked");
            public static PageId OccupationSearch { get; } = new PageId("occupationSearch");
        }

        public PageId Id { get; }

        public string PageTitle { get; }

        public CompositeSettings CompositeSettings { get; set; }

        protected CompositeViewModel(PageId pageId, string pageName)
        {
            Id = pageId;
            PageTitle = string.IsNullOrWhiteSpace(pageName) ? AppTitle : $"{pageName} | {AppTitle}";
        }

        public string GetElementId(string elementName, string instanceName)
        {
            return $"{Id}{elementName}{instanceName}";
        }
    }
}