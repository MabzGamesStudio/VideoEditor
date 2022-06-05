using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPreviewSettings : MonoBehaviour
{

	public ActionManager actionManager;

	public GameObject playPauseGameObject;

	private PlayPause playPause;

	public GameObject rewind;

	private PreviewSpeed rewindScript;

	public GameObject fastForward;

	private PreviewSpeed fastForwardScript;

	public GameObject skipForward;

	private PreviewSkip previewSkipForward;

	public GameObject skipBackward;

	private PreviewSkip previewSkipBackward;

	public GameObject slider;

	public GameObject timeline;

	public PreviewSlider previewSlider;

	public GameObject fastForwardText;

	public GameObject rewindText;

	public GameObject stepForwardText;

	public GameObject stepBackwardText;

	public GameObject startTimeText;

	private TimeText startTimeTextScript;

	public GameObject endTimeText;

	private TimeText endTimeTextScript;

	bool previewComplete;

	bool previewAtStart = true;

	private bool showVideoSettings = true;

	private TimeDisplay timeDisplay;

	private enum PlayState
	{
		Pause,
		Play,
		Rewind,
		Forward,
		StepForward,
		StepBackward
	}

	public enum TimeDisplay
	{
		Time,
		TimeTotal,
		Frame,
		FrameTotal
	}

	private PlayState currentState = PlayState.Pause;

	// Start is called before the first frame update
	void Start()
	{
		playPause = playPauseGameObject.GetComponent<PlayPause>();
		rewindScript = rewind.GetComponent<PreviewSpeed>();
		fastForwardScript = fastForward.GetComponent<PreviewSpeed>();
		previewSkipForward = skipForward.GetComponent<PreviewSkip>();
		previewSkipBackward = skipBackward.GetComponent<PreviewSkip>();
		startTimeTextScript = startTimeText.GetComponent<TimeText>();
		endTimeTextScript = endTimeText.GetComponent<TimeText>();
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.P))
		{
			showVideoSettings = !showVideoSettings;
			if (showVideoSettings)
			{
				ShowPreview();
			}
			else
			{
				HidePreview();
			}
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			actionManager.SaveAction();
		}

		if (!showVideoSettings && Input.GetKeyDown(KeyCode.Space))
		{
			if (actionManager.AtActionEnd())
			{
				actionManager.SetToStart();
				actionManager.PlayAction();
				currentState = PlayState.Play;
			}
			if (actionManager.GetActionPlaying())
			{
				actionManager.PauseAction();
				currentState = PlayState.Pause;
			}
			else
			{
				actionManager.PlayAction();
				currentState = PlayState.Play;
			}
		}

	}

	public void Play()
	{
		if (previewComplete)
		{
			previewComplete = false;
			actionManager.SetToStart();
		}
		actionManager.SetPlaySpeed(1f);
		actionManager.PlayAction();
		currentState = PlayState.Play;

		DeselectAll();
		playPause.Select();
	}

	public void Pause()
	{
		actionManager.PauseAction();
		currentState = PlayState.Pause;

		DeselectAll();
		playPause.Select();
	}

	public void HidePreview()
	{
		playPauseGameObject.SetActive(false);
		rewind.SetActive(false);
		fastForward.SetActive(false);
		skipBackward.SetActive(false);
		skipForward.SetActive(false);
		timeline.SetActive(false);
		slider.SetActive(false);
		fastForwardText.SetActive(false);
		rewindText.SetActive(false);
		stepForwardText.SetActive(false);
		stepBackwardText.SetActive(false);
		startTimeText.SetActive(false);
		endTimeText.SetActive(false);
	}

	public void ShowPreview()
	{
		playPauseGameObject.SetActive(true);
		rewind.SetActive(true);
		fastForward.SetActive(true);
		skipBackward.SetActive(true);
		skipForward.SetActive(true);
		timeline.SetActive(true);
		slider.SetActive(true);
		fastForwardText.SetActive(true);
		rewindText.SetActive(true);
		stepForwardText.SetActive(true);
		stepBackwardText.SetActive(true);
		startTimeText.SetActive(true);
		endTimeText.SetActive(true);
	}

	public void UpdatePlaySpeed(float speed)
	{
		if (speed < 0)
		{
			previewComplete = false;
		}
		else
		{
			previewAtStart = false;
		}
		if (currentState == PlayState.Rewind || currentState == PlayState.Forward)
		{
			actionManager.SetPlaySpeed(speed);
		}
		rewindScript.SetTextSpeed(speed);
		fastForwardScript.SetTextSpeed(speed);
	}

	public void UpdateSkipFrames(int frames)
	{
		previewSkipForward.SetTextFrames(frames);
		previewSkipBackward.SetTextFrames(frames);
	}

	public void SetPlaySpeed(float speed)
	{
		if (speed < 0)
		{
			previewComplete = false;
			if (previewAtStart)
			{
				previewAtStart = false;
				actionManager.SetToEnd();
			}
		}
		else
		{
			previewAtStart = false;
			if (previewComplete)
			{
				previewComplete = false;
				actionManager.SetToStart();
			}
		}
		actionManager.SetPlaySpeed(speed);
		actionManager.PlayAction();
		playPause.ForcePlay();
		currentState = speed < 0 ? PlayState.Rewind : PlayState.Forward;


		DeselectAll();
		if (speed < 0)
		{
			rewindScript.Select();
		}
		else
		{
			fastForwardScript.Select();
		}
	}

	public void StepPreview(int frames)
	{
		DeselectAll();
		if (frames < 0)
		{
			previewSkipBackward.Select();
			previewComplete = false;
		}
		else
		{
			previewSkipForward.Select();
			previewAtStart = false;
		}
		actionManager.SkipFrame(frames);
		actionManager.PauseAction();
		playPause.ForcePause();
		currentState = PlayState.Pause;
	}

	public void ResetPreview(bool atEnd)
	{
		if (atEnd)
		{
			previewComplete = true;
		}
		else
		{
			previewAtStart = true;
		}
		playPause.ForcePause();
		currentState = PlayState.Pause;
	}

	public void SetPreviewProgress(float progress, float totalTime, int totalFrames)
	{
		previewSlider.UpdateSlider(progress);
		startTimeTextScript.SetTotals(totalTime, totalFrames);
		endTimeTextScript.SetTotals(totalTime, totalFrames);
		if (timeDisplay == TimeDisplay.Time || timeDisplay == TimeDisplay.TimeTotal)
		{
			float seconds = progress * totalTime;
			startTimeTextScript.UpdateTime(seconds, totalTime);
			endTimeTextScript.UpdateTime(seconds, totalTime);
		}
		else
		{
			int currentFrame = Mathf.RoundToInt(progress * totalFrames);
			startTimeTextScript.UpdateFrames(currentFrame, totalFrames);
			endTimeTextScript.UpdateFrames(currentFrame, totalFrames);
		}
	}

	public void SetPreviewProgress(float currentTime, float totalTime)
	{
		startTimeTextScript.UpdateTime(currentTime, totalTime);
		endTimeTextScript.UpdateTime(currentTime, totalTime);
	}

	public void SetPreviewProgress(int currentFrame, int totalFrames)
	{
		startTimeTextScript.UpdateFrames(currentFrame, totalFrames);
		endTimeTextScript.UpdateFrames(currentFrame, totalFrames);
	}

	public void SetActionTime(float progress)
	{
		actionManager.SetActionProgress(progress);
		playPause.ForcePause();
		DeselectAll();
		currentState = PlayState.Pause;

		if (progress != 0f)
		{
			previewAtStart = false;
		}
		if (progress != 1f)
		{
			previewComplete = false;
		}
	}

	public void SetTimeDisplay(TimeDisplay timeDisplay)
	{
		this.timeDisplay = timeDisplay;
		startTimeTextScript.SetTimeDisplay(timeDisplay);
		endTimeTextScript.SetTimeDisplay(timeDisplay);
	}

	private void DeselectAll()
	{
		playPause.Deselect();
		previewSkipForward.Deselect();
		previewSkipBackward.Deselect();
		rewindScript.Deselect();
		fastForwardScript.Deselect();
	}
}
