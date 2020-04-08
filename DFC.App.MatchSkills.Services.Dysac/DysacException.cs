using System;
using System.Runtime.Serialization;

namespace DFC.App.MatchSkills.Services.Dysac
{
    [Serializable]
    public class DysacException : Exception
    {

        private DysacException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        public DysacException(string message)
            :base(message)
        { }
    }
}
