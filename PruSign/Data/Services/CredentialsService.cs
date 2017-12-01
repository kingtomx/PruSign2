using PruSign.Data.Entities;
using PruSign.Data.Interfaces;

namespace PruSign.Data.Services
{
    class CredentialsService : ICredentialsService
    {
        private readonly IServiceAsync<UserCredentials> _serviceUserCredentials;

        public CredentialsService(IServiceAsync<UserCredentials> serviceUserCredentials)
        {
            _serviceUserCredentials = serviceUserCredentials;
        }

        public async void SaveCredentials(string username, string password)
        {
            var dbItem = new UserCredentials()
            {
                Username = username,
                Password = password
            };
            await _serviceUserCredentials.Add(dbItem);
        }
    }
}
