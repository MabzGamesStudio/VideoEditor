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


			Process process = new Process();
			process.StartInfo = new ProcessStartInfo();
			process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.FileName = Application.dataPath + "/ffmpeg";


			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;



			UnityEngine.Debug.Log(process.StartInfo.FileName);

			process.StartInfo.Arguments = "-r 1 -i " +
				CommandLineDirectoryNoSpaces(Application.dataPath) +
				"/ImageFrames/Circle%01d.png -s 256x256 -framerate 1/2 " +
				CommandLineDirectoryNoSpaces(Application.dataPath) +
				"/ImageFrames/CircleAnimation.mp4";

			UnityEngine.Debug.Log("Argument: " + process.StartInfo.Arguments);

			process.Start();

			System.IO.StreamReader thing = process.StandardOutput;
			System.IO.StreamReader thing2 = process.StandardError;


			UnityEngine.Debug.Log("Standard Out: " + thing.ReadToEnd());
			UnityEngine.Debug.Log("Error Out: " + thing2.ReadToEnd());

			process.WaitForExit();



			UnityEngine.Debug.Log(thing.ReadToEnd());

		}

	}

	private string CommandLineDirectoryNoSpaces(string directory)
	{
		UnityEngine.Debug.Log(directory.Replace(" ", "'  '"));
		return directory.Replace(" ", "' '");
	}
}
