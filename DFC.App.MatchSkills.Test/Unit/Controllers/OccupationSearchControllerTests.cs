using System;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.ViewComponents.Choice;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{

    public class OccupationSearchControllerTests
    {
        private IDataProtectionProvider _dataProtectionProvider;
        private IDataProtector _dataProtector;
        private IOptions<ServiceTaxonomySettings> _settings;
        private ServiceTaxonomyRepository serviceTaxonomyRepository;
        
        [SetUp]
        public void Init()
        {
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(BaseController));
            _settings = Options.Create(new ServiceTaxonomySettings());
            _settings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/servicetaxonomy";
            _settings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _settings.Value.SearchOccupationInAltLabels ="true";
                  
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);


        }
        
        [Test]
        public void  When_OccupationSearch_Then_ShouldReturnOccupations()
        {
            var sut = new OccupationSearchController(_dataProtector,serviceTaxonomyRepository,_settings);
            
            var occupations =   sut.OccupationSearch("renewable");
            
            occupations.Result.Should().NotBeNull();
            occupations.Result.Count().Should().BeGreaterThan(0);
        }

        [Test]
        public void  When_OccupationSearchAuto_Then_ShouldReturnOccupations()
        {
            var sut = new OccupationSearchController(_dataProtector,serviceTaxonomyRepository,_settings);
            
            var occupations =   sut.OccupationSearchAuto("renewable");
            
            occupations.Result.Should().NotBeNull();

            occupations.Result.Should().HaveCountGreaterOrEqualTo(1);
        }


        public  Mock<HttpMessageHandler> GetMockMessageHandler(string contentToReturn="{'Id':1,'Value':'1'}", HttpStatusCode statusToReturn=HttpStatusCode.OK)
        {
            var handlerMock =  new Mock<HttpMessageHandler>(MockBehavior.Loose);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )

                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusToReturn,
                    Content = new StringContent(contentToReturn)
                })
                .Verifiable();
            return handlerMock;
        }
        class MockResult
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }
    }
}
