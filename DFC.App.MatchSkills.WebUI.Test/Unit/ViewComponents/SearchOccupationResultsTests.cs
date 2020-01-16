using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Configuration;
using DFC.App.MatchSkills.WebUI.ViewComponents.SearchOccupationResults;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.WebUI.Test.Unit.ViewComponents
{
    class SearchOccupationResultsTests
    {
        private IOptions<ServiceTaxonomySettings> _settings;
        

        [SetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _settings = Options.Create(config.GetSection("ServiceTaxonomySettings").Get<ServiceTaxonomySettings>());

        }


        [Test]
        public void SearchOccupationResultsTest() {
       
        // Arrange
        var stMock = new Mock<IServiceTaxonomySearcher>();
        var occupations = new Occupation[]
            {
                new Occupation("1", "Furniture 1", DateTime.Now),
                new Occupation("2", "Furniture 2", DateTime.Now),
                new Occupation("3", "Furniture 3", DateTime.Now),
                new Occupation("4", "Furniture 4", DateTime.Now),
                new Occupation("5", "Furniture 5", DateTime.Now),
                new Occupation("6", "Furniture 6", DateTime.Now),
                new Occupation("7", "Furniture 7", DateTime.Now),
                new Occupation("8", "Furniture 8", DateTime.Now),
                new Occupation("9", "Furniture 9", DateTime.Now),
                new Occupation("10", "Furniture 10", DateTime.Now),
                new Occupation("11", "Furniture 11", DateTime.Now),
                new Occupation("12", "Furniture 12", DateTime.Now)
            };
            stMock
                .Setup( st =>st.SearchOccupations<Occupation[]>("", "", "",false))
                .ReturnsAsync(occupations);

             
            var viewComponent = new SearchOccupationResults(stMock.Object, _settings);
          

            //Act
            var results = viewComponent.InvokeAsync("furniture",true);
            

            //Assert
            results.Should().NotBeNull();
            
        } 
    }
}
