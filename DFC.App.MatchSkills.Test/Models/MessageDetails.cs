using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Test.Models
{
    public class MessageDetails
    {
        public string Type { get; set; }
        public int LastLine { get; set; }
        public int LastColumn { get; set; }
        public int FirstColumn { get; set; }
        public SubType SubType { get; set; }
        public string Message { get; set; }
        public string Extract { get; set; }
    }

    public enum SubType
    {
        Undifined,
        Warning,
        Fatal,
        [JsonProperty("non-document-error")]
        NonDocumentError
    }
}
