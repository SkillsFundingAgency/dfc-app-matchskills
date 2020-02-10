using System.Collections.Generic;
using DFC.App.MatchSkills.ViewModels;

namespace DFC.App.MatchSkills.ViewComponents.Choice
{
    public class ChoiceComponentModel
    {
        public string Text { get; set; }
        public string ButtonText { get; set; }
        public string LinkText { get; set; }
        public List<RadioButtonModel> RadioButtons { get; set; }
        public string FormAction { get; set; }
        public CompositeViewModel ParentModel { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorSummaryMessage { get; set; }
        public bool HasError { get; set; }
    }
}
