using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PruSign.Data.DTOs;
using PruSign.Data.Entities;
using PruSign.Data.Interfaces;
using PruSign.Data.ViewModels;
using PruSign.Helpers;
using RestSharp;

namespace PruSign.Data.Services
{
    public class SignatureService : ISignatureService
    {
        private readonly IDeviceLogService _deviceLogService;
        private readonly IServiceAsync<Signature> _serviceSignature;

        public SignatureService(IDeviceLogService deviceLogService, IServiceAsync<Signature> serviceSignature)
        {
            _deviceLogService = deviceLogService;
            _serviceSignature = serviceSignature;
        }

        public async Task SendSignatures()
        {
            try
            {
                var signaturesToSend = await _serviceSignature.GetAll().Where(s => !s.Sent).ToListAsync();
                if (signaturesToSend.Count > 0)
                {
                    var client = new RestClient(Constants.BACKEND_HOST_NAME);
                    var request = new RestRequest("api/signature", Method.POST);
                    request.AddHeader("Content-Type", "application/json");

                    var data = new SignaturesDTO()
                    {
                        Imei = App.Current.Properties["IMEI"] as string,
                        User = App.Current.Properties["Username"] as string,
                        Signatures = signaturesToSend
                    };

                    request.AddJsonBody(data);

                    var response = await client.ExecuteTaskAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        foreach (var s in signaturesToSend)
                        {
                            s.Sent = true;
                            s.SentDate = DateTime.Now;
                            await _serviceSignature.Update(s);
                        }

                        //TO-DO Handle if there is data to store on Queries table (Call SaveIncomingQueries)


                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                _deviceLogService.Log(ex);
            }
        }

        public async void SaveSign(string name, string customerId, string documentId, string appName, string datetime)
        {
            try
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = System.IO.Path.Combine(documents, "temporalSignatures");

                var signatureFilePath = System.IO.Path.Combine(directoryname, "signature.png");
                var pointsFilePath = System.IO.Path.Combine(directoryname, "points.json");

                if (!System.IO.File.Exists(signatureFilePath) ||
                    !System.IO.File.Exists(pointsFilePath))
                {
                    throw new Exception("No signatures found to be sent");
                }

                byte[] signatureFile = System.IO.File.ReadAllBytes(signatureFilePath);
                String pointsString = System.IO.File.ReadAllText(pointsFilePath);

                var points = JsonConvert.DeserializeObject(pointsString);

                byte[] nameBytes = FileHelper.GetBytes(name);
                byte[] customerIdBytes = FileHelper.GetBytes(customerId);
                byte[] documentIdBytes = FileHelper.GetBytes(documentId);
                byte[] appNameBytes = FileHelper.GetBytes(appName);
                byte[] datetimeBytes = FileHelper.GetBytes(datetime);

                byte[] rv = new byte[signatureFile.Length + nameBytes.Length + customerIdBytes.Length + documentIdBytes.Length + appNameBytes.Length + datetimeBytes.Length];
                Buffer.BlockCopy(signatureFile, 0, rv, 0, signatureFile.Length);
                Buffer.BlockCopy(nameBytes, 0, rv, signatureFile.Length, nameBytes.Length);
                Buffer.BlockCopy(customerIdBytes, 0, rv, signatureFile.Length + nameBytes.Length, customerIdBytes.Length);
                Buffer.BlockCopy(documentIdBytes, 0, rv, signatureFile.Length + nameBytes.Length + customerIdBytes.Length, documentIdBytes.Length);
                Buffer.BlockCopy(appNameBytes, 0, rv, signatureFile.Length + nameBytes.Length + customerIdBytes.Length + documentIdBytes.Length, appNameBytes.Length);
                Buffer.BlockCopy(datetimeBytes, 0, rv, signatureFile.Length + nameBytes.Length + customerIdBytes.Length + documentIdBytes.Length + appNameBytes.Length, datetimeBytes.Length);

                String hash = FileHelper.SHA512StringHash(rv);

                var outboxFolder = System.IO.Path.Combine(documents, "outbox");
                System.IO.Directory.CreateDirectory(outboxFolder);

                SignatureViewModel sign = new SignatureViewModel
                {
                    points = points,
                    customerName = name,
                    customerId = customerId,
                    documentId = documentId,
                    applicationId = appName,
                    datetime = datetime,
                    image = signatureFile,
                    hash = hash
                };
                var json = JsonConvert.SerializeObject(sign);
                var filename = System.IO.Path.Combine(outboxFolder, DateTime.Now.Ticks + ".json");
                using (var streamWriter = new System.IO.StreamWriter(filename))
                {
                    streamWriter.Write(json);
                    streamWriter.Close();
                }

                Signature dbItem = new Signature()
                {
                    SignatureObject = json,
                    CustomerName = name,
                    DocumentId = documentId,
                    CustomerId = customerId,
                    ApplicationId = appName,
                };
                await _serviceSignature.Add(dbItem);
                System.IO.File.Delete(filename);
                System.IO.File.Delete(signatureFilePath);
                System.IO.File.Delete(pointsFilePath);
            }
            catch (Exception ex)
            {
                _deviceLogService.Log(ex);
            }
        }

        public async void CleanSentSignatures()
        {
            try
            {
                var signatures = await _serviceSignature.GetAll().Where(s => s.Sent).ToListAsync();

                foreach (var s in signatures)
                {
                    await _serviceSignature.Delete(s);
                }
            }
            catch (Exception ex)
            {
                _deviceLogService.Log(ex);
            }
        }
    }
}
