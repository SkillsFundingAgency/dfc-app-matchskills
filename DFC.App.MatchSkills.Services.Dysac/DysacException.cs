using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.MatchSkills.Services.Dysac
{
    public class DysacException : Exception
    {
        public DysacException(string message)
            :base(message)
        { }
    }
}
