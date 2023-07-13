using System;

namespace CanaryLauncherUpdate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			App app = new App();
			app.InitializeComponent();
			app.Run();
		}
	}
}
