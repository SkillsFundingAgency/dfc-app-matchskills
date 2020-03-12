using System.Text.RegularExpressions;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Helpers
{
    public static class MappingHelper
    {
        public static string GetIdFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "";
            
            int pos = url.LastIndexOf("/") + 1;
            if (pos <= 1 )
                return "";
            
            return url.Substring(pos, url.Length - pos);
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}
