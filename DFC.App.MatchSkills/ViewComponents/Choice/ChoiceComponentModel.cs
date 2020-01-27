using System.Collections.Generic;

namespace DFC.App.MatchSkills.ViewComponents.Choice
{
    public class ChoiceComponentModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string ButtonText { get; set; }
        public string LinkText { get; set; }
        public List<RadioButtonModel> RadioButtons { get; set; }

        public string PageId { get; set; }
    }
}
