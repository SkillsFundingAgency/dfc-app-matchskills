using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;

namespace DFC.App.MatchSkills.Application.Dysac
{
    public interface IDysacSessionWriter
    {
        Task<NewSessionResponse> CreateNewSession(string assessmentType);
    }
}
