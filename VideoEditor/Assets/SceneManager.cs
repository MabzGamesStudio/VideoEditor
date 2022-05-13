using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			cameraCapture.Capture();
			circle.transform.localPosition = new Vector3(2, 0, 0);
			cameraCapture.Capture();
			circle.transform.localPosition = new Vector3(4, 0, 0);
			cameraCapture.Capture();
		}
	}
}
