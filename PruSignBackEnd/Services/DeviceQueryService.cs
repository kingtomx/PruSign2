using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;
using PruSignBackEnd.DTOs;

namespace PruSignBackEnd.Services
{
    public class DeviceQueryService
    {
        private readonly Service<Answer> _serviceDeviceAnswers;
        private readonly PruSignContext _db = new PruSignContext();

        public DeviceQueryService()
        {
            _serviceDeviceAnswers = new Service<Answer>(_db);
        }

        public async Task<List<DeviceQueryDTO>> GetPendingQueries(string imei)
        {
            var queries = _serviceDeviceAnswers.GetAll().Where(q => q.Device.Imei.Equals(imei) && q.Status == 0);
            var queriesList = await queries.ToListAsync();

            var result = new List<DeviceQueryDTO>();

            foreach (var item in queriesList)
            {
                result.Add(new DeviceQueryDTO()
                {
                    QueryId = item.ID,
                    QueryCode = item.Question.Code,
                    QueryDescription = item.Question.Description
                });
            }
            return result;
        }
    }
}