using System;
using System.Windows;
using System.IO;
using System.Net;
using System.Windows.Threading;
using System.Net.Http;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LauncherConfig;

namespace CanaryLauncherUpdate
{
	public partial class SplashScreen : Window
	{
		static readonly string launcerConfigUrl = "https://raw.githubusercontent.com/Bazerg/Laucher/main/launcher_config.json";
		// Load informations of launcher_config.json file
		static readonly ClientConfig clientConfig = ClientConfig.LoadFromFile(launcerConfigUrl);
		
		static readonly string clientExecutableName = clientConfig.ClientExecutable;
		static readonly string urlClient = clientConfig.NewClientUrl;

		static readonly HttpClient httpClient = new HttpClient();
        readonly DispatcherTimer timer = new DispatcherTimer();

		private string GetLauncherPath(bool onlyBaseDirectory = false)
		{
            string launcherPath;
            if (string.IsNullOrEmpty(clientConfig.ClientFolder) || onlyBaseDirectory) {
				launcherPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
			} else {
				launcherPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/" + clientConfig.ClientFolder;
			}
			
			return launcherPath;
		}

		static string GetClientVersion(string path)
		{
			string json = path + "/launcher_config.json";
			StreamReader stream = new StreamReader(json);
			dynamic jsonString = stream.ReadToEnd();
			dynamic versionclient = JsonConvert.DeserializeObject(jsonString);
			foreach (string version in versionclient)
			{
				return version;
			}

			return "";
		}

		private void StartClient()
		{
			if (File.Exists(GetLauncherPath() + "/bin/" + clientExecutableName)) {
				Process.Start(GetLauncherPath() + "/bin/" + clientExecutableName);
				this.Close();
			}
		}

		public SplashScreen()
		{
			string newVersion = clientConfig.ClientVersion;
			if (newVersion == null)
			{
				this.Close();
			}

			// Start the client if the versions are the same
			if (File.Exists(GetLauncherPath(true) + "/launcher_config.json")) {
				Console.WriteLine("porra");
				string actualVersion = GetClientVersion(GetLauncherPath(true));
				if (newVersion == actualVersion && Directory.Exists(GetLauncherPath()) ) {
					StartClient();
				}
			}

			InitializeComponent();
			timer.Tick += new EventHandler(Timer_SplashScreen);
			timer.Interval = new TimeSpan(0, 0, 5);
			timer.Start();
		}

		public async void Timer_SplashScreen(object sender, EventArgs e)
		{
			var requestClient = new HttpRequestMessage(HttpMethod.Post, urlClient);
			var response = await httpClient.SendAsync(requestClient);
			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				this.Close();
			}

			if (!Directory.Exists(GetLauncherPath()))
			{
				Directory.CreateDirectory(GetLauncherPath());
			}
			MainWindow mainWindow = new MainWindow();
			this.Close();
			mainWindow.Show();
			timer.Stop();
		}
	}
}
