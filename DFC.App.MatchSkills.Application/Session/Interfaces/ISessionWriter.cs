using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Session.Interfaces
{
    public interface ISessionWriter
    {
        Task<string> CreateUserSession(string previousPage, string currentPage);
    }
}
