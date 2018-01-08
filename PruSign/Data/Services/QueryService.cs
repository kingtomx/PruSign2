using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PruSign.Data.Entities;
using PruSign.Data.Interfaces;

namespace PruSign.Data.Services
{
    public class QueryService : IQueryService
    {
        private readonly IServiceAsync<Query> _serviceQuery;
        private readonly IServiceAsync<Signature> _serviceSignature;


        public QueryService(IServiceAsync<Query> serviceQuery, IServiceAsync<Signature> serviceSignature)
        {
            _serviceQuery = serviceQuery;
            _serviceSignature = serviceSignature;
        }

        public async Task SaveIncomingQueries(List<Query> incomingQueries)
        {
            foreach (var item in incomingQueries)
            {
                var exists = await _serviceQuery.GetAll().Where(q => q.QueryId == item.QueryId).FirstOrDefaultAsync();
                if (exists == null)
                {
                    await _serviceQuery.Add(item);
                }
            }
        }

        public void SendQueries()
        {
            throw new NotImplementedException();
        }

        public async void RunQueries()
        {
            var pendingQueries = await _serviceQuery.GetAll().Where(q => q.Response == null).ToListAsync();
            foreach (var item in pendingQueries)
            {
                switch (item.QueryCode)
                {
                    case "CheckPendingSignatures":
                        item.Response = $"There are {SignaturesPending()} unsynced signatures";
                        await _serviceQuery.Update(item);
                        break;
                }
            }
        }

        // QUERIES

        public async Task<int> SignaturesPending()
        {
            var signatures = await _serviceSignature.GetAll().Where(s => !s.Sent).ToListAsync();
            return signatures.Count;
        }
    }
}
