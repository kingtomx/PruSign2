using System;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace PruSign
{
	public static class SenderUtil
	{


		public static void SendSign(String name, String customerId, String documentId, String appName, String datetime)
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var directoryname = System.IO.Path.Combine(documents, "temporalSignatures");

			if (!System.IO.File.Exists(System.IO.Path.Combine(directoryname, "signature.png")) ||
				!System.IO.File.Exists(System.IO.Path.Combine(directoryname, "points.json")))
			{
				throw new Exception("No signatures found to be sent");
			}

			byte[] signatureFile = System.IO.File.ReadAllBytes(System.IO.Path.Combine(directoryname, "signature.png"));
			String pointsString = System.IO.File.ReadAllText(System.IO.Path.Combine(directoryname, "points.json"));

			var points = JsonConvert.DeserializeObject(pointsString);

			byte[] nameBytes = GetBytes(name);
			byte[] customerIdBytes = GetBytes(customerId);
			byte[] documentIdBytes = GetBytes(documentId);
			byte[] appNameBytes = GetBytes(appName);
			byte[] datetimeBytes = GetBytes(datetime);

			byte[] rv = new byte[signatureFile.Length + nameBytes.Length + customerIdBytes.Length + documentIdBytes.Length + appNameBytes.Length + datetimeBytes.Length];
			System.Buffer.BlockCopy(signatureFile, 0, rv, 0, signatureFile.Length);
			System.Buffer.BlockCopy(nameBytes, 0, rv, signatureFile.Length, nameBytes.Length);
			System.Buffer.BlockCopy(customerIdBytes, 0, rv, signatureFile.Length + nameBytes.Length, customerIdBytes.Length);
			System.Buffer.BlockCopy(documentIdBytes, 0, rv, signatureFile.Length + nameBytes.Length + customerIdBytes.Length, documentIdBytes.Length);
			System.Buffer.BlockCopy(appNameBytes, 0, rv, signatureFile.Length + nameBytes.Length + customerIdBytes.Length + documentIdBytes.Length, appNameBytes.Length);
			System.Buffer.BlockCopy(datetimeBytes, 0, rv, signatureFile.Length + nameBytes.Length + customerIdBytes.Length + documentIdBytes.Length + appNameBytes.Length, datetimeBytes.Length);

			String hash = SHA512StringHash(rv);

			var outboxFolder = System.IO.Path.Combine(documents, "outbox");
			System.IO.Directory.CreateDirectory(outboxFolder);

			Signature sign = new Signature
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
			var filename = System.IO.Path.Combine(outboxFolder, System.DateTime.Now.Ticks + ".json");
			using (var streamWriter = new System.IO.StreamWriter(filename))
			{
				streamWriter.Write(json);
				streamWriter.Close();
			}

#if __IOS__
			PruSign.iOS.FileHelper fh = new iOS.FileHelper();
#endif
#if __ANDROID__
			PruSign.Droid.FileHelper fh = new Droid.FileHelper();
#endif
			SignatureDatabase db = new SignatureDatabase(fh.GetLocalFilePath("PruSign.db"));
			SignatureItem dbItem = new SignatureItem()
			{
				SignatureObject = json,
                CustomerName = name,
				DocumentId = documentId,
				DNI = customerId,
				AppId = appName,
				CreationTimeStamp = System.DateTime.Now.Ticks,
				Sent = false,
				SentTimeStamp = 0,
				Miscelanea = "{}"

			};
			db.SaveItemAsync(dbItem);
			System.IO.File.Delete(filename);
			//System.IO.File.Delete(System.IO.Path.Combine(directoryname, "signature.png"));
			//System.IO.File.Delete(System.IO.Path.Combine(directoryname, "points.json"));


		}



		private static string SHA512StringHash(byte[] input)
		{
			SHA512 shaM = new SHA512Managed();
			// Convert the input string to a byte array and compute the hash.
			byte[] data = shaM.ComputeHash(input);
			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}
			// Return the hexadecimal string.
			return sBuilder.ToString();
		}


		private static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}



	}
}
