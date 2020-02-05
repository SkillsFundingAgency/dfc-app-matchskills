using DFC.App.MatchSkills.Services.ServiceTaxonomy.Helpers;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Unit
{
    class MappingHelperTests
    {
        [TestCase( "http://data.europa.eu/esco/skill/7ff9cce1-6518-48db-8f59-2beb81508fb2", ExpectedResult="7ff9cce1-6518-48db-8f59-2beb81508fb2")]
        [TestCase( "", ExpectedResult="")]
        [TestCase( null, ExpectedResult="")]
        public string When_GetIdFromUrl_ReturnsGuidId(string url)
        {
            return  MappingHelper.GetIdFromUrl(url);
        }
    }
}
