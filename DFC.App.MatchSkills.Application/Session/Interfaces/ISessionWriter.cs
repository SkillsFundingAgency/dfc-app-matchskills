using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.Application.Session.Interfaces
{
    public interface ISessionWriter
    {
        string CreateUserSession(string previousPage, string currentPage);
    }
}
