using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class SceneManager : MonoBehaviour
{

	public Camera cam;

	public GameObject circle;

	private CameraCapture cameraCapture;

	// Start is called before the first frame update
	void Start()
	{
		cameraCapture = cam.GetComponent<CameraCapture>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			circle.transform.localPosition = new Vector3(0, 0, 0);
			cameraCapture.Capture();
			circle.transform.localPosition = new Vector3(1, 0, 0);
			cameraCapture.Capture();
			circle.transform.localPosition = new Vector3(1, 1, 0);
			cameraCapture.Capture();
			circle.transform.localPosition = new Vector3(0, 1, 0);
			cameraCapture.Capture();
			circle.transform.localPosition = new Vector3(-1, 1, 0);
			cameraCapture.Capture();
			circle.transform.localPosition = new Vector3(-1, 0, 0);
			cameraCapture.Capture();


			AnimationComputer.ComputeAnimation("ImageFrames", "Circle", new Vector2Int(256, 256), 1, 5);

		}

	}

}
