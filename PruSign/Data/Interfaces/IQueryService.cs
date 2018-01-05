using System.Net.Http;
using System.Threading.Tasks;

namespace PruSign.Data.Interfaces
{
    public interface IQueryService
    {
        Task<HttpResponseMessage> SaveIncomingQueries();
        void SendQueries();
    }
}
