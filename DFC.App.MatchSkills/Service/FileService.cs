using DFC.App.MatchSkills.Interfaces;

namespace DFC.App.MatchSkills.Service
{
    public class FileService : IFileService
    {
        public string ReadAllText(string path)
        {
           return  System.IO.File.ReadAllText(path);
        }
    }
}
