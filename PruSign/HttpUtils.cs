using System;
using System.Net.Http;

namespace PruSign
{
	public class HttpUtils
	{

		public static async void Post(string url, string path, string jsonData){
			try { 
				var client = new HttpClient();
				client.BaseAddress = new Uri(url);

				var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PostAsync(path, content);
				var result = await response.Content.ReadAsStringAsync();
				if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError){
					throw new Exception("StatusCode==500");
				}

			} catch (Exception ex) {
				throw ex;
			}

		}		



	}
}
