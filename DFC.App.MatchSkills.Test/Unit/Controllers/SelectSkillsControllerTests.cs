using System.Linq;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DFC.App.MatchSkills.ViewModels;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{

    public class SelectSkillsControllerTests
    {
        private IDataProtectionProvider _dataProtectionProvider;
        private IDataProtector _dataProtector;
        private IOptions<ServiceTaxonomySettings> _settings;
        private ServiceTaxonomyRepository serviceTaxonomyRepository;
        private IOptions<CompositeSettings> _compositeSettings;
        
        [SetUp]
        public void Init()
        {
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(BaseController));
            _settings = Options.Create(new ServiceTaxonomySettings());
            _compositeSettings = Options.Create(new CompositeSettings());
            _settings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/servicetaxonomy";
            _settings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _settings.Value.EscoUrl = "http://data.europa.eu/esco";

            _settings.Value.SearchOccupationInAltLabels ="true";
                  
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);


        }
        
        [TestCase("https://dev.api.nationalcareersservice.org.uk","key")]
        public async Task When_GetAllSkillsForOccupation_Then_ShouldReturnSkillsList(string url,string apiKey)
        {
            // ARRANGE
            const string skillsJson ="{skills:[" +
                                     "{\"type\": \"competency\",\"relationshipType\": \"essential\",\"skill\": \"collect biological data\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"}," +
                                     "{\"type\": \"competency\",\"relationshipType\": \"essential\",\"skill\": \"collect biological info\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"}," +
                                     "{\"type\": \"competency\",\"relationshipType\": \"optional\",\"skill\": \"collect samples\",\"alternativeLabels\": [\"biological data collection\", \"analysing biological records\"],\"uri\": \"aaa\"}]}";
            var handlerMock = GetMockMessageHandler(skillsJson,HttpStatusCode.OK);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            var vm = new SelectSkillsCompositeViewModel();
            
            // ACTs
            var result = await subjectUnderTest.GetAllSkillsForOccupation<Skill[]>(url,apiKey,"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197") ;
            vm.Skills = result.ToList();
            //var skills = vm.Skills.ToList();
            
            // ASSERT
            result.Should().NotBeNull();
            result.Length.Should().Be(3);


            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post 
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        
        }

        [Test]
        public void  When_GetOccupationIdFromName_Then_ShouldReturnOccupationId()
        {
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"Renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);
            var sut = new SelectSkillsController(_dataProtector,serviceTaxonomyRepository,_settings,_compositeSettings);
            
            var result =   sut.GetOccupationIdFromName("Renewable energy consultant");

            result.Result.Should().Be("114e1eff-215e-47df-8e10-45a5b72f8197");
            
        }
        
        #region CUIScaffoldingTests

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new SelectSkillsController(_dataProtector,serviceTaxonomyRepository,_settings, _compositeSettings);
            var result = controller.Head() as ViewResult;
           
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        
        [Test]
        public void WhenBodyCalled_ReturnHtml()
        {
            var controller = new SelectSkillsController(_dataProtector,serviceTaxonomyRepository,_settings, _compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }


       
        #endregion
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
    }
}
