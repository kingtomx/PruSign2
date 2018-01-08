using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PruSign.Data.Entities;

namespace PruSign.Data.Interfaces
{
    public interface IQueryService
    {
        Task SaveIncomingQueries(List<Query> incomingQueries);
        void SendQueries();
    }
}
