﻿using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewComponents.SearchOccupationResults;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;

namespace DFC.App.MatchSkills.Test.Unit.ViewComponents
{
    [TestFixture]
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
                new Occupation("1", "Furniture 1", DateTime.Now, "test"),
                new Occupation("2", "Furniture 2", DateTime.Now, "test"),
                new Occupation("3", "Furniture 3", DateTime.Now, "test"),
                new Occupation("4", "Furniture 4", DateTime.Now, "test"),
                new Occupation("5", "Furniture 5", DateTime.Now, "test"),
                new Occupation("6", "Furniture 6", DateTime.Now, "test"),
                new Occupation("7", "Furniture 7", DateTime.Now, "test"),
                new Occupation("8", "Furniture 8", DateTime.Now, "test"),
                new Occupation("9", "Furniture 9", DateTime.Now, "test"),
                new Occupation("10", "Furniture 10", DateTime.Now, "test"),
                new Occupation("11", "Furniture 11", DateTime.Now, "test"),
                new Occupation("12", "Furniture 12", DateTime.Now, "test"),
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


        [Test]
        public void WhenStOccupationSearchResultPropertiesUpdated_Then_ShouldStoreCorrectly()
        {
            var sut = new StOccupationSearchResult.OccupationSearchResult();
            sut.Occupations = new[]
            {
                new StOccupationSearchResult.StsOccupation
                {
                    Occupation = "Furniture1",
                    AlternativeLabels = new[] {"alt1", "alt2"},
                    Uri = "Uri",
                    LastModified = DateTime.Now,
                    Matches = new StOccupationSearchResult.Matches()
                    {
                        Occupation = new[] {"Furniture2", "Furniture3"},
                        AlternativeLabels = new[] {"Alt1", "Alt2"}
                    }
                }
            };
        
            sut.Occupations.Length.Should().Be(1);

        }


    }
}
