using System;
using System.Runtime.Serialization;

namespace DFC.App.MatchSkills.Services.Dysac
{
    
    public class DysacException : Exception
    {

       
        public DysacException(string message)
            :base(message)
        { }
    }
}
