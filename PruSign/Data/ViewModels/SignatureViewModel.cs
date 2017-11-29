using System;
namespace PruSign.Data.ViewModels
{
	public class SignatureViewModel
	{
		public byte[] image;
		public object points;
		public String datetime;
		public String customerName;
		public String customerId;
		public String documentId;
		public String applicationId;
		public String hash;
	}
}
