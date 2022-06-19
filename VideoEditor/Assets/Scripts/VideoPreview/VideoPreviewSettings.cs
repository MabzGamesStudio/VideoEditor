using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPreviewSettings : MonoBehaviour
{

	/// <summary>
	/// The script with overhead management of actions for elements on screen
	/// </summary>
	public ActionManager actionManager;

	/// <summary>
	/// Game object of the play/pause button
	/// </summary>
	public GameObject playPauseGameObject;

	/// <summary>
	/// Script of the play/pause button
	/// </summary>
	private PlayPause playPause;

	/// <summary>
	/// Game object of the rewind button
	/// </summary>
	public GameObject rewind;

	/// <summary>
	/// Script of the rewind button
	/// </summary>
	private PreviewSpeed rewindScript;

	/// <summary>
	/// Game object of the fast forward button
	/// </summary>
	public GameObject fastForward;

	/// <summary>
	/// Script of the fast forward button
	/// </summary>
	private PreviewSpeed fastForwardScript;

	/// <summary>
	/// Game object of the skip forward button
	/// </summary>
	public GameObject skipForward;

	/// <summary>
	/// Script of the skip forward button
	/// </summary>
	private PreviewSkip previewSkipForward;

	/// <summary>
	/// Game object of the skip backward button
	/// </summary>
	public GameObject skipBackward;

	/// <summary>
	/// Script of the skip backward button
	/// </summary>
	private PreviewSkip previewSkipBackward;

	/// <summary>
	/// Game object of the slider node
	/// </summary>
	public GameObject slider;

	/// <summary>
	/// Game object of the slider scale
	/// </summary>
	public GameObject timeline;

	/// <summary>
	/// Script of the slider node
	/// </summary>
	public PreviewSlider previewSlider;

	/// <summary>
	/// Game object of the fast forward text
	/// </summary>
	public GameObject fastForwardText;

	/// <summary>
	/// Game object of the rewind text
	/// </summary>
	public GameObject rewindText;

	/// <summary>
	/// Game object of the step forward text
	/// </summary>
	public GameObject stepForwardText;

	/// <summary>
	/// Game object of the step backward text
	/// </summary>
	public GameObject stepBackwardText;

	/// <summary>
	/// Game object of the start time text
	/// </summary>
	public GameObject startTimeText;

	/// <summary>
	/// Script of the start time text
	/// </summary>
	private TimeText startTimeTextScript;

	/// <summary>
	/// Game object of the end time text
	/// </summary>
	public GameObject endTimeText;

	/// <summary>
	/// Script of the end time text
	/// </summary>
	private TimeText endTimeTextScript;

	/// <summary>
	/// Whether the preview is at the end state
	/// </summary>
	private bool previewComplete;

	/// <summary>
	/// Whether the preview is at the start state
	/// </summary>
	private bool previewAtStart = true;

	/// <summary>
	/// Whether to show the GUI of the video preview
	/// </summary>
	private bool showVideoSettings = true;

	/// <summary>
	/// The type of format to display the time
	/// </summary>
	private TimeDisplay timeDisplay;

	/// <summary>
	/// 
	/// </summary>
	private PlayState currentState = PlayState.Pause;

	/// <summary>
	/// The different states of the preview
	/// </summary>
	private enum PlayState
	{
		Pause,
		Play,
		Rewind,
		Forward,
		StepForward,
		StepBackward
	}

	/// <summary>
	/// The different display types for the start/end text
	/// </summary>
	public enum TimeDisplay
	{
		Time,
		TimeTotal,
		Frame,
		FrameTotal
	}

	/// <summary>
	/// On the start of the game init the scrips of the game objects
	/// </summary>
	private void Start()
	{
		InitScripts();
	}

	/// <summary>
	/// Init the scripts of the GUI game objects
	/// </summary>
	private void InitScripts()
	{
		playPause = playPauseGameObject.GetComponent<PlayPause>();
		rewindScript = rewind.GetComponent<PreviewSpeed>();
		fastForwardScript = fastForward.GetComponent<PreviewSpeed>();
		previewSkipForward = skipForward.GetComponent<PreviewSkip>();
		previewSkipBackward = skipBackward.GetComponent<PreviewSkip>();
		startTimeTextScript = startTimeText.GetComponent<TimeText>();
		endTimeTextScript = endTimeText.GetComponent<TimeText>();
	}

	/// <summary>
	/// On every frame, check whether to toggle the preview, save the action,
	/// or apply a keyboard shortcut
	/// </summary>
	void Update()
	{
		CheckPreviewToggle();
		CheckSaveAction();
		CheckKeyboardShortcut();
	}

	/// <summary>
	/// Performs the toggle preview action if key 'p' pressed
	/// </summary>
	private void CheckPreviewToggle()
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
	}

	/// <summary>
	/// Performs the save action if key 's' pressed
	/// </summary>
	private void CheckSaveAction()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			actionManager.SaveAction();
		}
	}

	/// <summary>
	/// If the 'space' key is pressed, the video will pause, resume, or restart
	/// depending on the state
	/// </summary>
	private void CheckKeyboardShortcut()
	{

		// Only apply action if the video preview GUI is hidden
		if (!showVideoSettings && Input.GetKeyDown(KeyCode.Space))
		{

			// If the video is complete, then restart the video
			if (actionManager.AtActionEnd())
			{
				actionManager.SetToStart();
				actionManager.PlayAction();
				currentState = PlayState.Play;
			}

			// If the video is playing, then pause the video
			else if (actionManager.GetActionPlaying())
			{
				actionManager.PauseAction();
				currentState = PlayState.Pause;
			}

			// If the video is pause, then play the video
			else
			{
				actionManager.PlayAction();
				currentState = PlayState.Play;
			}
		}
	}

	/// <summary>
	/// Tells the aciton manager to play the action
	/// </summary>
	public void Play()
	{

		// If the video is at the end state, then restart the video
		if (previewComplete)
		{
			previewComplete = false;
			actionManager.SetToStart();
		}

		// Tell action manager to play video with normal speed
		actionManager.SetPlaySpeed(1f);
		actionManager.PlayAction();
		currentState = PlayState.Play;

		// Select only the play/pause button
		DeselectAll();
		playPause.Select();
	}

	/// <summary>
	/// Tells the action manager to pause the action
	/// </summary>
	public void Pause()
	{

		// Tell action manager to pause the action
		actionManager.PauseAction();
		currentState = PlayState.Pause;

		// Select only the play/pause button
		DeselectAll();
		playPause.Select();
	}

	/// <summary>
	/// Disables all video preview GUI gameobjects
	/// </summary>
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

	/// <summary>
	/// Enables all video preview GUI gameobjects
	/// </summary>
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

	/// <summary>
	/// Tells the GUI to change the play speed without altering play state
	/// </summary>
	/// <param name="speed">New speed value</param>
	public void UpdatePlaySpeed(float speed)
	{

		// If the speed is negative the preview is in rewind, so the preview
		// will not be complete
		if (speed < 0)
		{
			previewComplete = false;
		}

		// If the speed is positive the preview is in forward, so the preview
		// will not be at the start
		else
		{
			previewAtStart = false;
		}

		// If the video is playing, then tell the action manager to update the
		// speed
		if (currentState == PlayState.Rewind || currentState == PlayState.Forward)
		{
			actionManager.SetPlaySpeed(speed);
		}

		// Update the rewind and fast forward to the same value
		rewindScript.SetTextSpeed(speed);
		fastForwardScript.SetTextSpeed(speed);
	}

	/// <summary>
	/// Changes the number of frames per skip
	/// </summary>
	/// <param name="frames">New number of frames per skip</param>
	public void UpdateSkipFrames(int frames)
	{
		previewSkipForward.SetTextFrames(frames);
		previewSkipBackward.SetTextFrames(frames);
	}

	/// <summary>
	/// Tells the action manager play the action with a new speed
	/// </summary>
	/// <param name="speed">The new speed to play the action</param>
	public void SetPlaySpeed(float speed)
	{

		// If the speed is negative the video is in rewind
		if (speed < 0)
		{

			// The preview is in rewind so the preview is no longer complete
			previewComplete = false;

			// If the preview is at the start, reset the action to the end
			if (previewAtStart)
			{
				previewAtStart = false;
				actionManager.SetToEnd();
			}
		}

		// If the speed is negative the video is in fast forward
		else
		{

			// The preview is playing forward so the preview is no longer at the
			// start
			previewAtStart = false;

			// If the preview is at the end, reset the action to the start
			if (previewComplete)
			{
				previewComplete = false;
				actionManager.SetToStart();
			}
		}

		// Tell the action manager to play with given speed
		actionManager.SetPlaySpeed(speed);
		actionManager.PlayAction();

		// Tell the play/pause button to show play mode
		playPause.ForcePlay();
		currentState = speed < 0 ? PlayState.Rewind : PlayState.Forward;

		// Select only the rewind or fast forward button
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

	/// <summary>
	/// Tells the action manager to skip the action by the given frames
	/// </summary>
	/// <param name="frames">Number of frames to skip the video</param>
	public void StepPreview(int frames)
	{

		// Select only the step button
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

		// Tell the action manager to step the action
		actionManager.SkipFrame(frames);
		actionManager.PauseAction();
		playPause.ForcePause();
		currentState = PlayState.Pause;
	}

	/// <summary>
	/// Resets the action to the start or end
	/// </summary>
	/// <param name="atEnd">Whether to reset to the end state</param>
	public void ResetPreview(bool atEnd)
	{

		// Set preview at start/end variables depending on input
		if (atEnd)
		{
			previewComplete = true;
		}
		else
		{
			previewAtStart = true;
		}

		// Set play/pause button to pause state
		playPause.ForcePause();
		currentState = PlayState.Pause;
	}

	/// <summary>
	/// Sets the preview GUI to the correct preivew state based on progress
	/// </summary>
	/// <param name="progress">Progress of action from 0 to 1</param>
	/// <param name="totalTime">Total time for action</param>
	/// <param name="totalFrames">Total number of frames in action</param>
	public void SetPreviewProgress(float progress, float totalTime, int totalFrames)
	{

		// Update the slider to the correct progress position
		previewSlider.UpdateSlider(progress);

		// Set the total time and frames values in the time texts
		startTimeTextScript.SetTotals(totalTime, totalFrames);
		endTimeTextScript.SetTotals(totalTime, totalFrames);

		// Update the time value if in time mode
		if (timeDisplay == TimeDisplay.Time || timeDisplay == TimeDisplay.TimeTotal)
		{
			float seconds = progress * totalTime;
			startTimeTextScript.UpdateTime(seconds, totalTime);
			endTimeTextScript.UpdateTime(seconds, totalTime);
		}

		// Update the frame value if in frame mode
		else
		{
			int currentFrame = Mathf.RoundToInt(progress * totalFrames);
			startTimeTextScript.UpdateFrames(currentFrame, totalFrames);
			endTimeTextScript.UpdateFrames(currentFrame, totalFrames);
		}
	}

	/// <summary>
	/// Tells the time texts to set the time values
	/// </summary>
	/// <param name="currentTime">Current time through the action</param>
	/// <param name="totalTime">Total time for action</param>
	public void SetPreviewProgress(float currentTime, float totalTime)
	{
		startTimeTextScript.UpdateTime(currentTime, totalTime);
		endTimeTextScript.UpdateTime(currentTime, totalTime);
	}

	/// <summary>
	/// Tells the time text to set the frame values
	/// </summary>
	/// <param name="currentFrame">Current frame through the action</param>
	/// <param name="totalFrames">Total frames for action</param>
	public void SetPreviewProgress(int currentFrame, int totalFrames)
	{
		startTimeTextScript.UpdateFrames(currentFrame, totalFrames);
		endTimeTextScript.UpdateFrames(currentFrame, totalFrames);
	}

	/// <summary>
	/// Tells action manager to set action to specific progress value
	/// </summary>
	/// <param name="progress">Progress through action 0 to 1</param>
	public void SetActionTime(float progress)
	{

		// Tells the action manager to set the progress to the given value
		actionManager.SetActionProgress(progress);

		// Pauses the video
		playPause.ForcePause();
		currentState = PlayState.Pause;

		// Deselects all buttons
		DeselectAll();

		// Updates preview at start/end values if needed
		if (progress != 0f)
		{
			previewAtStart = false;
		}
		if (progress != 1f)
		{
			previewComplete = false;
		}
	}

	/// <summary>
	/// Sets the display type for the time texts
	/// </summary>
	/// <param name="timeDisplay">Time display type</param>
	public void SetTimeDisplay(TimeDisplay timeDisplay)
	{
		this.timeDisplay = timeDisplay;
		startTimeTextScript.SetTimeDisplay(timeDisplay);
		endTimeTextScript.SetTimeDisplay(timeDisplay);
	}

	/// <summary>
	/// Deselects all button GUI in the preview
	/// </summary>
	private void DeselectAll()
	{
		playPause.Deselect();
		previewSkipForward.Deselect();
		previewSkipBackward.Deselect();
		rewindScript.Deselect();
		fastForwardScript.Deselect();
	}

}
