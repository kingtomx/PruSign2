using System.Threading.Tasks;

namespace PruSign.Data.Interfaces
{
    public interface ISignatureService
    {
        Task SendSignatures();
        void SaveSign(string name, string customerId, string documentId, string appName, string datetime);
        void CleanSentSignatures();
    }
}
