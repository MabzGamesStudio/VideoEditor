using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The background sprite color for the video
/// </summary>
public class Background : MonoBehaviour
{

	/// <summary>
	/// The camera that captures the frame images
	/// </summary>
	public Camera captureCamera;

	/// <summary>
	/// The sprite renderer for the background image sprite
	/// </summary>
	private SpriteRenderer spriteRenderer;

	/// <summary>
	/// When the game starts set the background color to the background camera
	/// color
	/// </summary>
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Color backgroundColor = captureCamera.backgroundColor;
		backgroundColor.a = 1f;
		spriteRenderer.color = backgroundColor;
	}

}
