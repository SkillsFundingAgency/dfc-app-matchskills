using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace DFC.App.MatchSkills.Test.Helpers
{
    public static class CommonViewMethods
    {
        public static string ApplicationViewsPath
        {
            get
            {
                var testAssemblyPath = TestContext.CurrentContext.TestDirectory;
                var combinedFullPathToViews = Path.Combine(testAssemblyPath, @"..\..\..\..\DFC.App.MatchSkills.WebUI\Views");
                var applicationViewsPath = Path.GetFullPath(combinedFullPathToViews);
                return applicationViewsPath;
            }
        }

        public static IEnumerable<string> ViewFilenames
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

    }
}
