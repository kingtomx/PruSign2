using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PruSignBackEnd.Data.DB;
using PruSignBackEnd.Data.Entities;

namespace PruSignBackEnd.Services
{
    public class DeviceService
    {
        private readonly Service<Device> _serviceDevice;
        private readonly PruSignContext _db = new PruSignContext();


        public DeviceService()
        {
            _serviceDevice = new Service<Device>(_db);
        }

        public Device GetDevice(string imei)
        {
            return _serviceDevice.GetAll().FirstOrDefault(d => d.Imei.Equals(imei));
        }

        public void CreateDevice(Device device)
        {
            _serviceDevice.Add(new Device()
            {
                Imei = device.Imei,
                User = device.User
            });

            _db.SaveChanges();
        }
    }
}