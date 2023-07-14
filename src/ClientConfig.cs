using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LauncherConfig
{
    public class ClientConfig
	{
		public string ClientVersion { get; set; }
		public string LauncherVersion { get; set; }
		public bool ReplaceFolders { get; set; }
		public ReplaceFolderName[] ReplaceFolderName { get; set; }
		public string ClientFolder { get; set; }
		public string NewClientUrl { get; set; }
		public string NewConfigUrl { get; set; }
		public string ClientExecutable { get; set; }

		public static ClientConfig LoadFromFile(string url)
		{
			using (HttpClient client = new HttpClient())
			{
				Task<string> jsonTask = client.GetStringAsync(url);
				string jsonString = jsonTask.Result;
				return JsonConvert.DeserializeObject<ClientConfig>(jsonString);
			}
		}
	}

	public class ReplaceFolderName
	{
		public string Name { get; set; }
	}
}
