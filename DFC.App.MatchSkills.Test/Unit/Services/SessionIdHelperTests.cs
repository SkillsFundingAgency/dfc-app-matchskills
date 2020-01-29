using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.MatchSkills.Application.Session.Helpers;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Services
{
    public class SessionIdHelperTests
    {
        public class GenerateSessionIdTests
        {
            [Test]
            public void WhenSaltIsNull_ThrowError()
            {
                SessionIdHelper.GenerateSessionId(null, DateTime.Now);
            }
        }
    }
}
