using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the actions of all the elements
/// </summary>
public class ActionManager : MonoBehaviour
{

	/// <summary>
	/// The video preview settings that controls the action
	/// </summary>
	public VideoPreviewSettings videoPreviewSettings;

	/// <summary>
	/// The list of elements to apply the actions
	/// </summary>
	private List<Element> elements;

	/// <summary>
	/// Whether the action is playing on the update
	/// </summary>
	private bool actionPlaying;

	/// <summary>
	/// The current time through the action animation
	/// </summary>
	private float actionTime;

	/// <summary>
	/// The total time to complete the action for all elements
	/// </summary>
	private float TotalActionTime
	{
		get
		{
			if (_totalActionTime == -1)
			{
				for (int i = 0; i < elements.Count; i++)
				{
					_totalActionTime = Mathf.Max(_totalActionTime, elements[i].GetTotalActionTime());
				}
			}
			return _totalActionTime;
		}
		set
		{
			_totalActionTime = value;
		}
	}
	private float _totalActionTime = -1;

	/// <summary>
	/// The total number of frames in the animation
	/// </summary>
	private int TotalFrames
	{
		get
		{
			return Mathf.RoundToInt(fps * TotalActionTime);
		}
	}

	/// <summary>
	/// The number of frames per second
	/// </summary>
	public int fps;

	/// <summary>
	/// The dimensions of the video
	/// </summary>
	public Vector2Int frameDimensions;

	/// <summary>
	/// The camera capture to record the frame images
	/// </summary>
	public CameraCapture cameraCapture;

	/// <summary>
	/// The number of megabit stream of the video
	/// </summary>
	public int megaBitrate;

	/// <summary>
	/// The speed to play the action
	/// </summary>
	private float playSpeed = 1f;

	/// <summary>
	/// Update elements every frame
	/// </summary>
	void Update()
	{
		UpdateElementsIfPlaying();
	}

	/// <summary>
	/// Sets the elements for the complete action
	/// </summary>
	/// <param name="elements">The list of elements to add to the action</param>
	public void SetElements(List<Element> elements)
	{
		this.elements = elements;
		SetActionProgress(0f);
	}

	/// <summary>
	/// Whether the action is currently playing on update
	/// </summary>
	/// <returns>Whether the action is currently playing on update</returns>
	public bool GetActionPlaying()
	{
		return actionPlaying;
	}

	/// <summary>
	/// Whether the action is completed and at the end
	/// </summary>
	/// <returns>Whether the action is completed and at the end</returns>
	public bool AtActionEnd()
	{
		return actionTime == TotalActionTime;
	}

	/// <summary>
	/// Stops the play of action on update
	/// </summary>
	public void PauseAction()
	{
		actionPlaying = false;
	}

	/// <summary>
	/// Continues the play of action on update
	/// </summary>
	public void PlayAction()
	{
		actionPlaying = true;
	}

	/// <summary>
	/// Saves the full action to video format
	/// </summary>
	public void SaveAction()
	{

		// The total video time
		float videoTime = 0;

		// Sets the frame dimensions in pixels
		cameraCapture.SetDimensions(frameDimensions);

		// Sets padding digits based on number of frames
		cameraCapture.paddingDigits = 1 + (int)Mathf.Log10(TotalFrames);

		// Hides video preview to prevent it blocking the video
		videoPreviewSettings.HidePreview();

		// Takes an image of from the camera of each frame in the action
		for (int i = 0; i < TotalFrames; i++)
		{

			// Update all elements in action to be correctly displayed
			for (int j = 0; j < elements.Count; j++)
			{
				elements[j].UpdateElement(videoTime);
			}

			// Take frame image and increment action time by 1 frame
			cameraCapture.Capture();
			videoTime += 1f / fps;
		}

		// Compute video from frames captured
		AnimationComputer.ComputeAnimation("VideoData", "CircleAnimation", frameDimensions, fps, TotalFrames, megaBitrate);

		// Show video preview settings
		videoPreviewSettings.ShowPreview();
	}

	/// <summary>
	/// Skip video time by given frames
	/// </summary>
	/// <param name="numFrames">Number of frames to skip</param>
	public void SkipFrame(int numFrames)
	{

		// Increase action time by given frames
		actionTime += ((float)numFrames) / fps;

		// Update elements to new frame
		UpdateElementsToActionTime();
	}

	/// <summary>
	/// Sets speed of action in update
	/// </summary>
	/// <param name="speed">Play speed (1 is normal speed)</param>
	public void SetPlaySpeed(float speed)
	{
		playSpeed = speed;
	}

	/// <summary>
	/// Sets the action progress to the start
	/// </summary>
	public void SetToStart()
	{
		actionTime = 0f;
		videoPreviewSettings.SetPreviewProgress(actionTime / TotalActionTime, TotalActionTime, TotalFrames);
	}

	/// <summary>
	/// Sets the action progress to the end
	/// </summary>
	public void SetToEnd()
	{
		actionTime = TotalActionTime;
		videoPreviewSettings.SetPreviewProgress(actionTime / TotalActionTime, TotalActionTime, TotalFrames);
	}

	/// <summary>
	/// Updates the action on update if action should be playing
	/// </summary>
	private void UpdateElementsIfPlaying()
	{

		// If the action should be playing
		if (actionPlaying)
		{

			// Increment action time by update time and play speed
			actionTime += Time.deltaTime * playSpeed;

			// Update elements to new time
			UpdateElementsToActionTime();
		}
	}

	/// <summary>
	/// Set action time to specific percentage progress
	/// </summary>
	/// <param name="progress">Progress of action from 0 to 1</param>
	public void SetActionProgress(float progress)
	{
		actionTime = progress * TotalActionTime;
		actionPlaying = false;
		UpdateElementsToActionTime();
	}

	/// <summary>
	/// Update all elements to the action time
	/// </summary>
	private void UpdateElementsToActionTime()
	{

		// Action start and end edge cases
		if (actionTime < 0)
		{
			actionTime = 0;
		}
		if (actionTime > TotalActionTime)
		{
			actionTime = TotalActionTime;
		}

		// Update elements within action
		for (int i = 0; i < elements.Count; i++)
		{
			elements[i].UpdateElement(actionTime);
		}

		// Update video preview settings
		videoPreviewSettings.SetPreviewProgress(actionTime / TotalActionTime, TotalActionTime, TotalFrames);
		if (actionTime == 0)
		{
			videoPreviewSettings.ResetPreview(false);
		}
		if (actionTime == TotalActionTime)
		{
			videoPreviewSettings.ResetPreview(true);
		}
	}

}
