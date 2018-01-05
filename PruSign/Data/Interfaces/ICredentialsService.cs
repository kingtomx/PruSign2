using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PruSign.Data.Interfaces
{
    public interface ICredentialsService
    {
        void SaveCredentials(string username, string password);
        Task<string> GetUsername();
    }
}
