using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.Logging.OSLog.LoggingTest.TestMaui;

public partial class App : Application
{
	private LoggerFactory _factory;
	public App(ILoggerFactory factory)
	{
		InitializeComponent();
		_factory = (LoggerFactory)factory;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage(_factory));
	}
}