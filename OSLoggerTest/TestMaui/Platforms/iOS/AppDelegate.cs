using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace com.mahonkin.tim.LoggingTest.TestMaui;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
