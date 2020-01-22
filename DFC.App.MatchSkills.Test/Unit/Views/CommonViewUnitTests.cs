using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using FluentAssertions;
using HtmlAgilityPack;

namespace DFC.App.MatchSkills.Test.Unit.Views
{
    [TestFixture]
    public class CommonViewUnitTests
    {
        private static IEnumerable<string> ActionElements
        {
            get { return new[]
            {
                "govukButton",
                "govukStartButton",
                "govukSecondaryButton",
                "govukWarningButton",
                "govukButtonLink",
                "govukStartButtonLink",
                "govukBackLink",
                "govukAutoComplete",
                "govukRadioButton",
                "govukCheckbox"
            }; }
        }

        private static string ApplicationViewsPath
        {
            get
            {
                var testAssemblyPath = TestContext.CurrentContext.TestDirectory;
                var combinedFullPathToViews = Path.Combine(testAssemblyPath, @"..\..\..\..\DFC.App.MatchSkills\Views");
                var applicationViewsPath = Path.GetFullPath(combinedFullPathToViews);
                return applicationViewsPath;
            }
        }

        private static IEnumerable<string> ViewFilenames
        {
            get
            {
                var viewFileNames = new List<string>();
                foreach (var filename in Directory.EnumerateFiles(ApplicationViewsPath, "*.cshtml", SearchOption.AllDirectories))
                {
                    viewFileNames.Add(filename);
                }

                return viewFileNames;
            }
        }

        private static List<TestCaseData> ActionElementTestCases
        {
            get
            {
                var tcs = new List<TestCaseData>();
                foreach (var viewFileName in ViewFilenames)
                {
                    tcs.Add(new TestCaseData(viewFileName, ActionElements));
                }

                return tcs;
            }
        }

        [TestCaseSource(nameof(ActionElementTestCases))]
        public void When_ViewExistsInSolution_Then_ActionElementsMustHaveIdAttribute(string viewFileName, IEnumerable<string> elementsWhichShouldHaveIds)
        {
            var doc = new HtmlDocument();
            doc.Load(viewFileName);

            TestContext.Out.WriteLine("View: " + viewFileName);

            foreach (var elementName in elementsWhichShouldHaveIds)
            {
                var xpath = $"//{elementName.ToLower()}";
                var elementNodes = doc.DocumentNode.SelectNodes(xpath);
                if (null != elementNodes && elementNodes.Count > 0)
                {
                    TestContext.Out.Write($"{elementName} : {elementNodes.Count} node(s) present in the HTML.");

                    foreach (var elementNode in elementNodes)
                    {
                        var because = $"element {elementName} on line {elementNode.Line} position {elementNode.LinePosition} should have an id attribute";
                        elementNode.Attributes["id"].Should().NotBeNull(because);
                    }
                }
                else
                {
                    TestContext.Out.Write($"{elementName} : No nodes present in the HTML.");
                }
            }
        }
    }
}
