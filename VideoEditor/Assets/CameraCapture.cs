using System.IO;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{
	public int fileCounter;
	public KeyCode screenshotKey;
	public string saveLocation;
	public string saveName;
	private Camera Camera
	{
		get
		{
			if (!_camera)
			{
				_camera = gameObject.GetComponent<Camera>();
			}
			return _camera;
		}
	}
	private Camera _camera;

	private void LateUpdate()
	{
		if (Input.GetKeyDown(screenshotKey))
		{
			//Capture();
		}
	}

	public void Capture()
	{

		RenderTexture activeRenderTexture = RenderTexture.active;
		RenderTexture.active = Camera.targetTexture;

		Camera.Render();

		Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
		image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
		image.Apply();
		RenderTexture.active = activeRenderTexture;

		byte[] bytes = image.EncodeToPNG();
		Destroy(image);

		File.WriteAllBytes(Application.dataPath + "/" + saveLocation + "/" + saveName + fileCounter + ".png", bytes);
		fileCounter++;
	}
}