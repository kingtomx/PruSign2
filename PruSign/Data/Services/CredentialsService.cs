using System;
using System.Collections.Generic;
using System.Text;
using PruSign.Data.Entities;
using PruSign.Data.Interfaces;

namespace PruSign.Data.Services
{
    class CredentialsService : ICredentialsService
    {
        private IDBService _db;

        public CredentialsService(IDBService db)
        {
            _db = db;
        }

        public async void SaveCredentials(string username, string password)
        {
            var serviceUserCredentials = new ServiceAsync<UserCredentials>(_db);
            UserCredentials dbItem = new UserCredentials()
            {
                Username = username,
                Password = password
            };
            await serviceUserCredentials.Add(dbItem);
        }
    }
}
