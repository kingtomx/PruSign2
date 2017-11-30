using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign.Data.Interfaces
{
    public interface ICredentialsService
    {
        void SaveCredentials(string username, string password);
    }
}
