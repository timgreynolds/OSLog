using System;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.Logging.OSLog.LoggingTest.TestMaui;

public partial class MainPage : ContentPage
{
	private IntPtr _logPtr = OSLogger.Create(nameof(LoggingTest), nameof(MainPage));
	private Logger<MainPage> _logger;

	public MainPage(LoggerFactory factory)
	{
		InitializeComponent();
		OSLogger.LogDebug(_logPtr, $"MainPage component initialize.");
		_logger = (Logger<MainPage>)factory.CreateLogger<MainPage>();
		_logger.LogDebug($"MainPage component initialize. This should not appear in Console.");
		foreach (string level in Enum.GetNames<LogLevel>())
		{
			LevelPicker.Items.Add(level);
		}
		OSLogger.LogDebug(_logPtr, $"Picker Item List set. {LevelPicker.Items.Count} Items added");
		_logger.LogDebug($"Picker Item List set. {LevelPicker.Items.Count} Items added This should not appear in Console.");
		LevelPicker.SelectedIndex = LevelPicker.Items.Count - 1;
	}

	private void OnMessageClicked(object sender, EventArgs e)
	{
		if (Enum.TryParse<LogLevel>(LevelPicker.SelectedItem.ToString(), out LogLevel selectedLevel))
		{
			OSLogger.Log(_logPtr, OSLogger.GetOsLogType(selectedLevel), $"Level: {selectedLevel} logged");
		}
		else
		{
			OSLogger.LogWarning(_logPtr, $"Could not parse log level {selectedLevel}");
		}
	}
}

