using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Models;
using DFC.Personalisation.Common.Extensions;

namespace DFC.App.MatchSkills.ViewModels
{
    public abstract class CompositeViewModel
    {
        public static string AppTitle => "Discover your skills and careers";

        public class PageId
        {
            private PageId(string value)
            {
                Throw.IfNullOrWhiteSpace(value, nameof(value));
                Value = value.Trim();
            }

            public override string ToString()
            {
                return Value;
            }

            public string Value { get; }

            public static PageId Home { get; } = new PageId("home");
            public static PageId Worked { get; } = new PageId("worked");
            public static PageId OccupationSearch { get; } = new PageId("occupationSearch");
            public static PageId SkillsBasket { get; } = new PageId("basket");
            public static PageId Route { get; } = new PageId("route");
            public static PageId Matches { get; } = new PageId("matches");
            public static PageId MoreSkills { get; } = new PageId("moreSkills");
            public static PageId MatchDetails { get; } = new PageId("matchDetails");
            public static  PageId SelectSkills { get; } = new PageId("selectSkills");
            public static  PageId MoreJobs { get; } = new PageId("moreJobs");
            
        }

        public class PageRegion
        {
            private PageRegion(string value)
            {
                Value = value;
            }
            public override string ToString()
            {
                return Value;
            }
            public string Value { get; }
            public static PageRegion Body { get; } = new PageRegion("body");

        }

        public PageId Id { get; }

        
        public string PageTitle { get; }
        public string PageHeading { get; }

        public CompositeSettings CompositeSettings { get; set; }

        protected CompositeViewModel(PageId pageId, string pageHeading)
        {
            Id = pageId;
            PageHeading = pageHeading;
            PageTitle = string.IsNullOrWhiteSpace(pageHeading) ? AppTitle : $"{pageHeading} | {AppTitle}";
        }

        #region Helpers

        public string GetElementId(string elementName, string instanceName)
        {
            Throw.IfNullOrWhiteSpace(elementName, nameof(elementName));
            Throw.IfNullOrWhiteSpace(instanceName, nameof(instanceName));
            elementName = elementName.FirstCharToUpper().Trim();
            instanceName = instanceName.FirstCharToUpper().Trim();
            return $"{Id}{elementName}{instanceName}";
        }

        public string NounForNumber(int number, string singularNoun, string pluralNoun)
        {
            if (1 == number)
            {
                return singularNoun;
            }
            else
            {
                return pluralNoun;
            }
        }

        #endregion Helpers
    }
}