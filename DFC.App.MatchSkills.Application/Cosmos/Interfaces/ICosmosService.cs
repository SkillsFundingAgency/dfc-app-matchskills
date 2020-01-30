using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;
using Microsoft.Azure.Cosmos;

namespace DFC.App.MatchSkills.Application.Cosmos.Interfaces
{
    public interface ICosmosService
    {
        Task CreateDocumentAsync(object item);
    }
}
