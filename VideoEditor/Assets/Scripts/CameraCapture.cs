using System.IO;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{

	/// <summary>
	/// The image counter (starts at 0 and goes to # of frames - 1)
	/// </summary>
	public int fileCounter;

	/// <summary>
	/// The save location within the Asset folder
	/// </summary>
	public string saveLocation;

	/// <summary>
	/// The name of the frames and video
	/// </summary>
	public string saveName;

	/// <summary>
	/// The number of 0s to pad to the start of the file counter
	/// </summary>
	public int paddingDigits;

	/// <summary>
	/// The camera to take the capture
	/// </summary>
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

	/// <summary>
	/// Sets the dimensions of the capture in pixels
	/// </summary>
	/// <param name="newDimension">Dimensions of the capture in pixels</param>
	public void SetDimensions(Vector2Int newDimension)
	{
		Camera.targetTexture = new RenderTexture(
			newDimension.x, newDimension.y, 24);
	}

	/// <summary>
	/// Takes a capture from the camera of the screen and saves the 
	/// </summary>
	public void Capture()
	{

		// Sets the texture for the camera to save the image
		RenderTexture activeRenderTexture = RenderTexture.active;
		RenderTexture.active = Camera.targetTexture;
		Camera.Render();

		// Takes the camera capture and converts it to an image
		Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
		image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
		image.Apply();
		RenderTexture.active = activeRenderTexture;

		// Encodes the image to a PNG
		byte[] bytes = image.EncodeToPNG();
		Destroy(image);

		// Gets the padded number for the frame number
		string paddedFileCounter = "";
		for (int i = fileCounter.ToString().Length; i < paddingDigits; i++)
		{
			paddedFileCounter += "0";
		}
		paddedFileCounter += fileCounter.ToString();

		// Writes the image data to the specified file path and name
		File.WriteAllBytes(Application.dataPath + "/" + saveLocation + "/" + saveName + paddedFileCounter + ".png", bytes);
		fileCounter++;
	}

	// TODO: Implement this to save space for frames when video has been created
	/// <summary>
	/// Deletes the image frame files
	/// </summary>
	public void DeleteFrames()
	{

	}
}