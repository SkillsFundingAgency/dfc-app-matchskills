using DFC.App.MatchSkills.Services.JobProfile.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Services.JobProfile.Interfaces;
using Dfc.ProviderPortal.Packages;

namespace DFC.App.MatchSkills.Services.JobProfile.Services
{
    public class LmiService : ILmiReader
    {
        private readonly ILogger _log;
        private readonly LmiApiSettings _lmiApiSettings;
        private readonly Uri _getSocSearchUri, _getWfPredictUri;
        private readonly RestClient _client;
        public LmiService(ILogger log, LmiApiSettings lmiApiSettings, RestClient client)
        {
            Throw.IfNull(log, nameof(log));
            Throw.IfNull(lmiApiSettings.ApiUrl, nameof(lmiApiSettings.ApiUrl));
            Throw.IfNull(client, nameof(client));

            _log = log;
            _lmiApiSettings = lmiApiSettings;
            _client = client;

            _getSocSearchUri = lmiApiSettings.GetSocSearchUri();
            _getWfPredictUri = lmiApiSettings.GetWfPredictUri();
        }

        public async Task<IEnumerable<SocSearchResults>> SocSearch(SocSearchCriteria criteria)
        {
            try
            {
                if (criteria == null)
                {
                    throw new ArgumentException("SearchCriteria cannot be null.", nameof(criteria));
                }
                if (string.IsNullOrWhiteSpace(criteria.SearchCriteria))
                {
                    throw new ArgumentException("SearchCriteria cannot be empty.", nameof(criteria));
                }

                return await _client.Get<IEnumerable<SocSearchResults>>(
                    _getSocSearchUri.AbsoluteUri + criteria.SearchCriteria);
            }
            catch (ArgumentException aex)
            {
                _log.LogError(aex.Message, aex);
                throw;
            }
            catch(HttpRequestException hre)
            {
                _log.LogError(hre.Message, hre);
                throw;
            }
            catch(Exception ex)
            {
                _log.LogError(ex.Message, ex);
                throw;
            }
        }
        public async Task<WorkingFuturesSearchResults> WFSearch(WorkingFuturesRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentException("Request cannot be null.", nameof(request));
                }
                if (request.SocCode == 0)
                {
                    throw new ArgumentException("SocCode cannot be zero.", nameof(request.SocCode));
                }

                if (request.SocCode < 0)
                {
                    throw new ArgumentException("SocCode cannot be less than zero.", nameof(request.SocCode));
                }
                return await _client.Get<WorkingFuturesSearchResults>(
                    _getWfPredictUri.AbsoluteUri + request.SocCode);
            }
            catch (ArgumentException aex)
            {
                _log.LogError(aex.Message, aex);
                throw;
            }
            catch(HttpRequestException hre)
            {
                _log.LogError(hre.Message, hre);
                throw;
            }
            catch(Exception ex)
            {
                _log.LogError(ex.Message, ex);
                throw;
            }
        }
    }
    internal static class LmiServiceSettingsExtensions
    {
        internal static Uri GetSocSearchUri(this LmiApiSettings extendee)
        {
            var uri = new Uri(extendee.ApiUrl);
            var trimmed = uri.AbsoluteUri.TrimEnd('/');
            return new Uri($"{trimmed}{LmiEndpoints.SocSearchPath}{LmiEndpoints.SocSearchQueryString}");
        }
        internal static Uri GetWfPredictUri(this LmiApiSettings extendee)
        {
            var uri = new Uri(extendee.ApiUrl);
            var trimmed = uri.AbsoluteUri.TrimEnd('/');
            return new Uri($"{trimmed}{LmiEndpoints.WfPredictSearchPath}{LmiEndpoints.WfPredictQueryString}");
        }
    }
    internal static class LmiEndpoints
    {
        internal const string SocSearchPath = "/soc/search";
        internal const string SocSearchQueryString = "?q=";
        internal const string WfPredictSearchPath = "/wf/predict";
        internal const string WfPredictQueryString = "?soc=";
    }
}
