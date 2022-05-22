using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

	List<Element> elements;

	public Element circle1;
	public Element circle2;

	bool actionGoing;

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

	public int fps;

	public Vector2Int frameDimensions;

	public CameraCapture cameraCapture;

	public int megaBitrate;

	// Start is called before the first frame update
	void Start()
	{

		elements = new List<Element>();
		elements.Add(circle1);
		elements.Add(circle2);

	}

	private void ShowAction()
	{
		actionGoing = true;
		actionTime = 0;
	}



	private void SaveAction()
	{

		float videoTime = 0;
		int totalFrames = Mathf.RoundToInt(fps * TotalActionTime);

		cameraCapture.SetDimensions(frameDimensions);

		for (int i = 0; i < totalFrames; i++)
		{
			for (int j = 0; j < elements.Count; j++)
			{
				elements[j].UpdateElement(videoTime);
			}



			cameraCapture.Capture();

			videoTime += 1f / fps;
		}


		AnimationComputer.ComputeAnimation("VideoData", "CircleAnimation", frameDimensions, fps, totalFrames, megaBitrate);

	}

	// Update is called once per frame
	void Update()
	{
		if (actionGoing)
		{
			actionTime += Time.deltaTime;

			for (int i = 0; i < elements.Count; i++)
			{
				elements[i].UpdateElement(actionTime);
			}
			if (TotalActionTime < actionTime)
			{
				actionGoing = false;
			}
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			ShowAction();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			SaveAction();
		}

	}
}
