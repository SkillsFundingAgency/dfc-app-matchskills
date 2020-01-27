using DFC.App.MatchSkills.Test.Helpers;
using DFC.App.MatchSkills.Test.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills;

namespace DFC.App.MatchSkills.Test.Integration
{
    public class HtmlValidationTests
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public HtmlValidationTests()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        private static List<HtmlValidationTestModel> HtmlValidationTestCases()
        {
            var values = new List<HtmlValidationTestModel>();
            var viewFileNames = CommonViewMethods.ApplicationViewsPath;
            var tcs = new List<TestCaseData>();
            foreach (var viewFileName in CommonViewMethods.ViewFilenames.Where(x=>!x.Contains("_ViewImports")))
            {
                var fileBreakDowns = viewFileName.Replace(".cshtml", "").Replace(viewFileNames+"\\", "").Split('\\');

                values.Add(new HtmlValidationTestModel
                {
                    Segment = fileBreakDowns[1],
                    View = fileBreakDowns[0]
                });
            }

            return values;
        }

        [TestCaseSource(nameof(HtmlValidationTestCases))]
        [Ignore("While we decide how best to do it. Occupation search is causing network errors")]
        public async Task WhenSegmentRequested_ThenTheReturnedHTMLShouldPassW3Validation(HtmlValidationTestModel testModel)
        {

            TestContext.Out.Write($"View:{testModel.View}, Segments: {testModel.Segment} : Html Validation Started \n");

            var resp = await _client.GetAsync($"/{testModel.Segment}/{testModel.View}");
            var responseString = await resp.Content.ReadAsStringAsync();

            var validationResponse = string.Empty;

            using (var validationClient = new HttpClient())
            {
                validationClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");

                var content = new StringContent(responseString, Encoding.UTF8, "text/html");

                var uri = new Uri("https://validator.w3.org/nu/?out=json");
                var response = await validationClient.PostAsync(uri, content);

                validationResponse = await response.Content.ReadAsStringAsync();
            }

            var result = JsonConvert.DeserializeObject<ValidationResult>(validationResponse);

            var errors = result.Messages.Where(x => x.SubType == SubType.Fatal).ToList();

            if (errors.Any())
            {
                var because = $"View:{testModel.View}, Segments: {testModel.Segment} has the following errors\n";
                because = errors.Aggregate(because, (current, error) => current + $"\n {error.Message}, on component {error.Extract}");
                errors.Any().Should().BeFalse(because);
            }
            else
            {
                TestContext.Out.Write($"View:{testModel.View}, Segments: {testModel.Segment} : Html Validation Passed\n");
            }

            TestContext.Out.Write($"View:{testModel.View}, Segments: {testModel.Segment} : Html Validation Finished");

        }
    }
}
