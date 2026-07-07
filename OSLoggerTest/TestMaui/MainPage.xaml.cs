using System;
using com.mahonkin.tim.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.LoggingTest.TestMaui;

public partial class MainPage : ContentPage
{
	private IntPtr _logPtr = OSLogger.Create(nameof(LoggingTest), nameof(MainPage));

	public MainPage()
	{
		InitializeComponent();
		OSLogger.LogDebug(_logPtr, $"MainPage component initialize.");
		foreach (string level in Enum.GetNames<LogLevel>())
		{
			LevelPicker.Items.Add(level);
		}
		OSLogger.LogDebug(_logPtr, $"Picker Item List set. {LevelPicker.Items.Count} Items added");
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

