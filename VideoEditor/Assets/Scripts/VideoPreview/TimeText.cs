using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeText : MonoBehaviour
{

	/// <summary>
	/// Script that manages all video preview GUI
	/// </summary>
	public VideoPreviewSettings videoPreviewSettings;

	/// <summary>
	/// Text mesh pro UGUI display text
	/// </summary>
	private TextMeshProUGUI text;

	/// <summary>
	/// Type of time display
	/// </summary>
	private VideoPreviewSettings.TimeDisplay timeDisplay;

	/// <summary>
	/// Whether this text represents the start time
	/// </summary>
	public bool isStartTime;

	/// <summary>
	/// Time to display in seconds
	/// </summary>
	private float timeSeconds;

	/// <summary>
	/// Total time for action in seconds
	/// </summary>
	private float timeTotal;

	/// <summary>
	/// Currrent frame to display
	/// </summary>
	private int timeFrames;

	/// <summary>
	/// Total frames in action
	/// </summary>
	private int framesTotal;

	/// <summary>
	/// Updates the type of time display
	/// </summary>
	/// <param name="timeDisplay">Type of time display to use</param>
	public void SetTimeDisplay(VideoPreviewSettings.TimeDisplay timeDisplay)
	{
		this.timeDisplay = timeDisplay;
	}

	/// <summary>
	/// Sets the total time in seconds and total time in frames
	/// </summary>
	/// <param name="totalSeconds">Total seconds in action</param>
	/// <param name="totalFrames">Total frames in action</param>
	public void SetTotals(float totalSeconds, int totalFrames)
	{
		timeTotal = totalSeconds;
		framesTotal = totalFrames;
	}

	/// <summary>
	/// Updates the time to the given input
	/// </summary>
	/// <param name="currentTime">Current time through action</param>
	/// <param name="totalTime">Total time of action</param>
	public void UpdateTime(float currentTime, float totalTime)
	{

		// Update time variables
		timeSeconds = currentTime;
		timeTotal = totalTime;

		// String version for time variables
		string currentTimeDisplay;
		string totalTimeDisplay = "";

		// If the total time varible will be used in the display
		if (timeDisplay == VideoPreviewSettings.TimeDisplay.TimeTotal)
		{

			// Total time minutes and seconds
			int totalTimeDisplayMinutes = (int)totalTime / 60;
			float totalTimeDisplaySeconds = totalTime % 60f;

			// Displays seconds string as XX.XX seconds
			string displaySeconds = totalTimeDisplaySeconds.ToString("0.00");
			if (displaySeconds.Length == 4)
			{
				displaySeconds = "0" + displaySeconds;
			}

			// Total time is MINUTES:SECONDS.SECONDS_FRACTION (X:XX.XX)
			totalTimeDisplay = "\n/" + totalTimeDisplayMinutes + ":" +
				displaySeconds;
		}

		// If the text is the end time, then the time is the time from the start
		if (!isStartTime)
		{
			currentTime = Mathf.Abs(currentTime - totalTime);
		}

		// Current time in minutes and seconds
		int currentTimeDisplayMinutes = (int)currentTime / 60;
		float currentTimeDisplaySeconds = currentTime % 60f;

		// If using complex display, show the time with milliseconds and total
		// time
		if (timeDisplay == VideoPreviewSettings.TimeDisplay.TimeTotal)
		{
			currentTimeDisplay = currentTimeDisplayMinutes + ":" +
				currentTimeDisplaySeconds.ToString("0.00");
		}

		// If using simple display, show only the current time in minutes and
		// whole seconds
		else
		{
			currentTimeDisplay = currentTimeDisplayMinutes + ":" +
				((int)currentTimeDisplaySeconds).ToString("D2");
		}

		// If start time set text to display time, and if end time set time to
		// be negative
		if (isStartTime)
		{
			text.text = currentTimeDisplay + totalTimeDisplay;
		}
		else
		{
			text.text = "-" + currentTimeDisplay + totalTimeDisplay;
		}
	}

	/// <summary>
	/// Updates the time in frames to the input
	/// </summary>
	/// <param name="currentFrame">Current frame in action</param>
	/// <param name="totalFrames">Total frames in action</param>
	public void UpdateFrames(int currentFrame, int totalFrames)
	{

		// Update frame time variables
		timeFrames = currentFrame;
		framesTotal = totalFrames;

		// If end text, use negative frames
		if (!isStartTime)
		{
			currentFrame -= totalFrames;
		}

		// If complex display, set text to current frames / total frames
		if (timeDisplay == VideoPreviewSettings.TimeDisplay.FrameTotal)
		{
			text.text = currentFrame.ToString() + "\n/" +
				totalFrames.ToString();
		}

		// If simple display, set text to current frame
		else
		{
			text.text = currentFrame.ToString();
		}
	}

	/// <summary>
	/// When the text box collision 2D is clicked, toggle the display type
	/// </summary>
	private void OnMouseDown()
	{
		switch (timeDisplay)
		{

			// If on Time, change to TimeTotal
			case VideoPreviewSettings.TimeDisplay.Time:
				videoPreviewSettings.SetTimeDisplay(
					VideoPreviewSettings.TimeDisplay.TimeTotal);
				videoPreviewSettings.SetPreviewProgress(
					timeSeconds, timeTotal);
				break;

			// If on TimeTotal, change to Frame
			case VideoPreviewSettings.TimeDisplay.TimeTotal:
				videoPreviewSettings.SetTimeDisplay(
					VideoPreviewSettings.TimeDisplay.Frame);
				videoPreviewSettings.SetPreviewProgress(
					timeSeconds, timeTotal);
				break;

			// If on Frame, change to FrameTotal
			case VideoPreviewSettings.TimeDisplay.Frame:
				videoPreviewSettings.SetTimeDisplay(
					VideoPreviewSettings.TimeDisplay.FrameTotal);
				videoPreviewSettings.SetPreviewProgress(
					timeFrames, framesTotal);
				break;

			// If on FrameTotal, change to Time
			case VideoPreviewSettings.TimeDisplay.FrameTotal:
				videoPreviewSettings.SetTimeDisplay(
					VideoPreviewSettings.TimeDisplay.Time);
				videoPreviewSettings.SetPreviewProgress(
					timeFrames, framesTotal);
				break;
		}
	}

}
