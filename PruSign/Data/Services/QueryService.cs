using System;
using System.Net.Http;
using System.Threading.Tasks;
using PruSign.Data.Interfaces;

namespace PruSign.Data.Services
{
    public class QueryService : IQueryService
    {
        public async Task<HttpResponseMessage> SaveIncomingQueries()
        {
            throw new NotImplementedException();
        }

        public void SendQueries()
        {
            throw new NotImplementedException();
        }
    }
}
