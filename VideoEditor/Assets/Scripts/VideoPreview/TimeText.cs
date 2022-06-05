using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeText : MonoBehaviour
{

	public VideoPreviewSettings videoPreviewSettings;

	private TextMeshProUGUI text;

	private VideoPreviewSettings.TimeDisplay timeDisplay;

	public bool isStartTime;

	private float timeSeconds;

	private float timeTotal;

	private int timeFrames;

	private int framesTotal;

	public void SetTimeDisplay(VideoPreviewSettings.TimeDisplay timeDisplay)
	{
		this.timeDisplay = timeDisplay;
	}

	public void SetTotals(float totalSeconds, int totalFrames)
	{
		timeTotal = totalSeconds;
		framesTotal = totalFrames;
	}

	public void UpdateTime(float currentTime, float totalTime)
	{


		timeSeconds = currentTime;
		timeTotal = totalTime;


		string currentTimeDisplay;
		string totalTimeDisplay = "";

		if (timeDisplay == VideoPreviewSettings.TimeDisplay.TimeTotal)
		{
			int totalTimeDisplayMinutes = (int)totalTime / 60;
			float totalTimeDisplaySeconds = totalTime % 60f;

			string displaySeconds = totalTimeDisplaySeconds.ToString("0.00");
			if (displaySeconds.Length == 4)
			{
				displaySeconds = "0" + displaySeconds;
			}

			totalTimeDisplay = "\n/" + totalTimeDisplayMinutes + ":" + displaySeconds;
		}

		if (!isStartTime)
		{
			currentTime = Mathf.Abs(currentTime - totalTime);
		}

		int currentTimeDisplayMinutes = (int)currentTime / 60;
		float currentTimeDisplaySeconds = currentTime % 60f;

		if (timeDisplay == VideoPreviewSettings.TimeDisplay.TimeTotal)
		{
			currentTimeDisplay = currentTimeDisplayMinutes + ":" + currentTimeDisplaySeconds.ToString("0.00");
		}
		else
		{
			currentTimeDisplay = currentTimeDisplayMinutes + ":" + ((int)currentTimeDisplaySeconds).ToString("D2");
		}

		if (isStartTime)
		{
			text.text = currentTimeDisplay + totalTimeDisplay;
		}
		else
		{
			text.text = "-" + currentTimeDisplay + totalTimeDisplay;
		}

	}

	public void UpdateFrames(int currentFrame, int totalFrames)
	{

		timeFrames = currentFrame;
		framesTotal = totalFrames;

		if (!isStartTime)
		{
			currentFrame -= totalFrames;
		}

		if (timeDisplay == VideoPreviewSettings.TimeDisplay.FrameTotal)
		{
			text.text = currentFrame.ToString() + "\n/" + totalFrames.ToString();
		}
		else
		{
			text.text = currentFrame.ToString();
		}
	}

	private void OnMouseDown()
	{
		switch (timeDisplay)
		{
			case VideoPreviewSettings.TimeDisplay.Time:
				videoPreviewSettings.SetTimeDisplay(VideoPreviewSettings.TimeDisplay.TimeTotal);
				videoPreviewSettings.SetPreviewProgress(timeSeconds, timeTotal);
				break;
			case VideoPreviewSettings.TimeDisplay.TimeTotal:
				videoPreviewSettings.SetTimeDisplay(VideoPreviewSettings.TimeDisplay.Frame);
				videoPreviewSettings.SetPreviewProgress(timeSeconds, timeTotal);
				break;
			case VideoPreviewSettings.TimeDisplay.Frame:
				videoPreviewSettings.SetTimeDisplay(VideoPreviewSettings.TimeDisplay.FrameTotal);
				videoPreviewSettings.SetPreviewProgress(timeFrames, framesTotal);
				break;
			case VideoPreviewSettings.TimeDisplay.FrameTotal:
				videoPreviewSettings.SetTimeDisplay(VideoPreviewSettings.TimeDisplay.Time);
				videoPreviewSettings.SetPreviewProgress(timeFrames, framesTotal);
				break;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
