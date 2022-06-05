using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

	public VideoPreviewSettings videoPreviewSettings;

	List<Element> elements;

	bool actionPlaying;

	float actionTime;

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

	private int TotalFrames
	{
		get
		{
			return Mathf.RoundToInt(fps * TotalActionTime);
		}
	}

	public int fps;

	public Vector2Int frameDimensions;

	public CameraCapture cameraCapture;

	public int megaBitrate;

	private float playSpeed = 1f;

	public void SetElements(List<Element> elements)
	{
		this.elements = elements;
		SetActionProgress(0f);
	}

	public bool GetActionPlaying()
	{
		return actionPlaying;
	}

	public bool AtActionEnd()
	{
		return actionTime == TotalActionTime;
	}

	public void PauseAction()
	{
		actionPlaying = false;
	}

	public void PlayAction()
	{
		actionPlaying = true;
	}

	public void SaveAction()
	{

		float videoTime = 0;

		cameraCapture.SetDimensions(frameDimensions);

		for (int i = 0; i < TotalFrames; i++)
		{
			for (int j = 0; j < elements.Count; j++)
			{
				elements[j].UpdateElement(videoTime);
			}

			cameraCapture.Capture();

			videoTime += 1f / fps;
		}


		AnimationComputer.ComputeAnimation("VideoData", "CircleAnimation", frameDimensions, fps, TotalFrames, megaBitrate);

	}

	public void SkipFrame(int numFrames)
	{
		actionTime += ((float)numFrames) / fps;
		if (actionTime < 0)
		{
			actionTime = 0;
		}
		if (actionTime > TotalActionTime)
		{
			actionTime = TotalActionTime;
		}
		for (int i = 0; i < elements.Count; i++)
		{
			elements[i].UpdateElement(actionTime);
		}

		videoPreviewSettings.SetPreviewProgress(actionTime / TotalActionTime, TotalActionTime, TotalFrames);

		if (actionTime == 0 && numFrames < 0)
		{
			videoPreviewSettings.ResetPreview(false);
		}

		if (actionTime == TotalActionTime && numFrames > 0)
		{
			videoPreviewSettings.ResetPreview(true);
		}
	}

	public void SetPlaySpeed(float speed)
	{
		playSpeed = speed;
	}

	public void SetToStart()
	{
		actionTime = 0f;
		videoPreviewSettings.SetPreviewProgress(actionTime / TotalActionTime, TotalActionTime, TotalFrames);
	}

	public void SetToEnd()
	{
		actionTime = TotalActionTime;
		videoPreviewSettings.SetPreviewProgress(actionTime / TotalActionTime, TotalActionTime, TotalFrames);
	}

	// Update is called once per frame
	void Update()
	{
		if (actionPlaying)
		{
			actionTime += Time.deltaTime * playSpeed;

			if (actionTime < 0)
			{
				actionTime = 0f;
			}

			if (actionTime > TotalActionTime)
			{
				actionTime = TotalActionTime;
			}

			for (int i = 0; i < elements.Count; i++)
			{
				elements[i].UpdateElement(actionTime);
			}

			videoPreviewSettings.SetPreviewProgress(actionTime / TotalActionTime, TotalActionTime, TotalFrames);

			if (actionTime == 0 && playSpeed < 0)
			{
				actionPlaying = false;
				videoPreviewSettings.ResetPreview(false);
			}

			if (actionTime == TotalActionTime && playSpeed > 0)
			{
				actionPlaying = false;
				videoPreviewSettings.ResetPreview(true);
			}
		}

	}

	public void SetActionProgress(float progress)
	{
		actionTime = progress * TotalActionTime;
		actionPlaying = false;
		if (actionTime < 0f)
		{
			actionTime = 0f;
		}

		if (actionTime > TotalActionTime)
		{
			actionTime = TotalActionTime;
		}

		for (int i = 0; i < elements.Count; i++)
		{
			elements[i].UpdateElement(actionTime);
		}

		videoPreviewSettings.SetPreviewProgress(actionTime / TotalActionTime, TotalActionTime, TotalFrames);

		Debug.Log(actionTime);

		if (actionTime == 0f)
		{
			actionPlaying = false;
			videoPreviewSettings.ResetPreview(false);
		}

		if (actionTime == TotalActionTime)
		{
			actionPlaying = false;
			videoPreviewSettings.ResetPreview(true);
		}
	}
}
