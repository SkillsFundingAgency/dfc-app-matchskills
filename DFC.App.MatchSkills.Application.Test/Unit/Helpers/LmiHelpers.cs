﻿using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Test.Unit.Helpers
{
    public static class LmiHelpers
    {
        public static string SuccessfulLmiCall()
        {
            #region Result
            return
                "{  \"soc\": 2815,  \"breakdown\": \"region\",  \"predictedEmployment\": [    {      \"year\": 2016,      \"breakdown\": [        {          \"code\": 12,          \"name\": \"Northern Ireland\",          \"employment\": 1902,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 9,          \"name\": \"North East (England)\",          \"employment\": 3776,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 1,          \"name\": \"London\",          \"employment\": 9538,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 10,          \"name\": \"Wales\",          \"employment\": 3720,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 4,          \"name\": \"South West (England)\",          \"employment\": 7241,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 7,          \"name\": \"Yorkshire and the Humber\",          \"employment\": 8110,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 8,          \"name\": \"North West (England)\",          \"employment\": 10400        },        {          \"code\": 2,          \"name\": \"South East (England)\",          \"employment\": 12323        },        {          \"code\": 11,          \"name\": \"Scotland\",          \"employment\": 8100,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 5,          \"name\": \"West Midlands (England)\",          \"employment\": 7365,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 6,          \"name\": \"East Midlands (England)\",          \"employment\": 5822,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 3,          \"name\": \"East of England\",          \"employment\": 8262,          \"note\": \"Small sample size. Data may be inaccurate.\"        }      ]    },    {      \"year\": 2017,      \"breakdown\": [        {          \"code\": 5,          \"name\": \"West Midlands (England)\",          \"employment\": 7390,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 2,          \"name\": \"South East (England)\",          \"employment\": 12409        },        {          \"code\": 11,          \"name\": \"Scotland\",          \"employment\": 8193,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 8,          \"name\": \"North West (England)\",          \"employment\": 10435        },        {          \"code\": 3,          \"name\": \"East of England\",          \"employment\": 8328,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 9,          \"name\": \"North East (England)\",          \"employment\": 3771,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 6,          \"name\": \"East Midlands (England)\",          \"employment\": 5861,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 12,          \"name\": \"Northern Ireland\",          \"employment\": 1898,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 7,          \"name\": \"Yorkshire and the Humber\",          \"employment\": 8139,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 1,          \"name\": \"London\",          \"employment\": 9605,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 4,          \"name\": \"South West (England)\",          \"employment\": 7292,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 10,          \"name\": \"Wales\",          \"employment\": 3719,          \"note\": \"Small sample size. Data may be inaccurate.\"        }      ]    },    {      \"year\": 2018,      \"breakdown\": [        {          \"code\": 9,          \"name\": \"North East (England)\",          \"employment\": 3771,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 3,          \"name\": \"East of England\",          \"employment\": 8364,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 12,          \"name\": \"Northern Ireland\",          \"employment\": 1890,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 6,          \"name\": \"East Midlands (England)\",          \"employment\": 5874,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 7,          \"name\": \"Yorkshire and the Humber\",          \"employment\": 8153,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 1,          \"name\": \"London\",          \"employment\": 9642,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 4,          \"name\": \"South West (England)\",          \"employment\": 7319,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 10,          \"name\": \"Wales\",          \"employment\": 3712,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 2,          \"name\": \"South East (England)\",          \"employment\": 12844        },        {          \"code\": 11,          \"name\": \"Scotland\",          \"employment\": 8261,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 5,          \"name\": \"West Midlands (England)\",          \"employment\": 7385,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 8,          \"name\": \"North West (England)\",          \"employment\": 10423        }      ]    },    {      \"year\": 2019,      \"breakdown\": [        {          \"code\": 4,          \"name\": \"South West (England)\",          \"employment\": 7343,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 10,          \"name\": \"Wales\",          \"employment\": 3707,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 7,          \"name\": \"Yorkshire and the Humber\",          \"employment\": 8178,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 2,          \"name\": \"South East (England)\",          \"employment\": 12498        },        {          \"code\": 8,          \"name\": \"North West (England)\",          \"employment\": 10408        },        {          \"code\": 5,          \"name\": \"West Midlands (England)\",          \"employment\": 7373,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 11,          \"name\": \"Scotland\",          \"employment\": 8310,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 6,          \"name\": \"East Midlands (England)\",          \"employment\": 5881,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 3,          \"name\": \"East of England\",          \"employment\": 8390,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 12,          \"name\": \"Northern Ireland\",          \"employment\": 1883,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 1,          \"name\": \"London\",          \"employment\": 9666,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 9,          \"name\": \"North East (England)\",          \"employment\": 3771,          \"note\": \"Small sample size. Data may be inaccurate.\"        }      ]    },    {      \"year\": 2020,      \"breakdown\": [        {          \"code\": 3,          \"name\": \"East of England\",          \"employment\": 8411,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 11,          \"name\": \"Scotland\",          \"employment\": 8356,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 9,          \"name\": \"North East (England)\",          \"employment\": 3779,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 6,          \"name\": \"East Midlands (England)\",          \"employment\": 5891,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 12,          \"name\": \"Northern Ireland\",          \"employment\": 1879,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 1,          \"name\": \"London\",          \"employment\": 9690,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 7,          \"name\": \"Yorkshire and the Humber\",          \"employment\": 8219,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 4,          \"name\": \"South West (England)\",          \"employment\": 7370,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 10,          \"name\": \"Wales\",          \"employment\": 3712,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 5,          \"name\": \"West Midlands (England)\",          \"employment\": 7382,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 2,          \"name\": \"South East (England)\",          \"employment\": 12533        },        {          \"code\": 8,          \"name\": \"North West (England)\",          \"employment\": 10401        }      ]    },    {      \"year\": 2021,      \"breakdown\": [        {          \"code\": 7,          \"name\": \"Yorkshire and the Humber\",          \"employment\": 8269,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 1,          \"name\": \"London\",          \"employment\": 9684,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 4,          \"name\": \"South West (England)\",          \"employment\": 7391,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 2,          \"name\": \"South East (England)\",          \"employment\": 12561        },        {          \"code\": 10,          \"name\": \"Wales\",          \"employment\": 3717,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 11,          \"name\": \"Scotland\",          \"employment\": 8411,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 5,          \"name\": \"West Midlands (England)\",          \"employment\": 7399,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 8,          \"name\": \"North West (England)\",          \"employment\": 10393        },        {          \"code\": 9,          \"name\": \"North East (England)\",          \"employment\": 3790,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 3,          \"name\": \"East of England\",          \"employment\": 8437,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 12,          \"name\": \"Northern Ireland\",          \"employment\": 1875,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 6,          \"name\": \"East Midlands (England)\",          \"employment\": 5904,          \"note\": \"Small sample size. Data may be inaccurate.\"        }      ]    },    {      \"year\": 2022,      \"breakdown\": [        {          \"code\": 8,          \"name\": \"North West (England)\",          \"employment\": 10395        },        {          \"code\": 2,          \"name\": \"South East (England)\",          \"employment\": 12594        },        {          \"code\": 5,          \"name\": \"West Midlands (England)\",          \"employment\": 7423,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 11,          \"name\": \"Scotland\",          \"employment\": 8472,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 6,          \"name\": \"East Midlands (England)\",          \"employment\": 5921,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 3,          \"name\": \"East of England\",          \"employment\": 8468,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 12,          \"name\": \"Northern Ireland\",          \"employment\": 1872,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 1,          \"name\": \"London\",          \"employment\": 9683,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 9,          \"name\": \"North East (England)\",          \"employment\": 3804,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 4,          \"name\": \"South West (England)\",          \"employment\": 7413,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 10,          \"name\": \"Wales\",          \"employment\": 3724,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 7,          \"name\": \"Yorkshire and the Humber\",          \"employment\": 8328,          \"note\": \"Small sample size. Data may be inaccurate.\"        }      ]    },    {      \"year\": 2023,      \"breakdown\": [        {          \"code\": 9,          \"name\": \"North East (England)\",          \"employment\": 3815,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 12,          \"name\": \"Northern Ireland\",          \"employment\": 1872,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 6,          \"name\": \"East Midlands (England)\",          \"employment\": 5933,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 7,          \"name\": \"Yorkshire and the Humber\",          \"employment\": 8400,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 1,          \"name\": \"London\",          \"employment\": 9714,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 10,          \"name\": \"Wales\",          \"employment\": 3735,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 4,          \"name\": \"South West (England)\",          \"employment\": 7447,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 5,          \"name\": \"West Midlands (England)\",          \"employment\": 7446,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 8,          \"name\": \"North West (England)\",          \"employment\": 10395        },        {          \"code\": 2,          \"name\": \"South East (England)\",          \"employment\": 12658        },        {          \"code\": 11,          \"name\": \"Scotland\",          \"employment\": 8501,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 3,          \"name\": \"East of England\",          \"employment\": 8509,          \"note\": \"Small sample size. Data may be inaccurate.\"        }      ]    },    {      \"year\": 2024,      \"breakdown\": [        {          \"code\": 2,          \"name\": \"South East (England)\",          \"employment\": 12720        },        {          \"code\": 10,          \"name\": \"Wales\",          \"employment\": 3747,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 8,          \"name\": \"North West (England)\",          \"employment\": 10394        },        {          \"code\": 5,          \"name\": \"West Midlands (England)\",          \"employment\": 7465,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 11,          \"name\": \"Scotland\",          \"employment\": 8542,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 6,          \"name\": \"East Midlands (England)\",          \"employment\": 5942,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 12,          \"name\": \"Northern Ireland\",          \"employment\": 1871,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 3,          \"name\": \"East of England\",          \"employment\": 8549,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 9,          \"name\": \"North East (England)\",          \"employment\": 3826,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 4,          \"name\": \"South West (England)\",          \"employment\": 7477,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 7,          \"name\": \"Yorkshire and the Humber\",          \"employment\": 8475,          \"note\": \"Small sample size. Data may be inaccurate.\"        },        {          \"code\": 1,          \"name\": \"London\",          \"employment\": 9742,          \"note\": \"Small sample size. Data may be inaccurate.\"        }      ]    }  ]}";

            #endregion
        }
        public static Mock<HttpMessageHandler> GetMockMessageHandler(string contentToReturn="{'Id':1,'Value':'1'}", HttpStatusCode statusToReturn=HttpStatusCode.OK)
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