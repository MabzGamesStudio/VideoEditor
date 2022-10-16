using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zooms the text into existence
/// </summary>
public class TextZoomElement : Element
{

	/// <summary>
	/// Time before the text zooms in
	/// </summary>
	public float waitTime;

	/// <summary>
	/// Time it takes for the text to zoom
	/// </summary>
	public float zoomTime;

	/// <summary>
	/// The position of the text in the world
	/// </summary>
	public Vector2 position;

	/// <summary>
	/// Initializes the still path position and zoom of the text
	/// </summary>
	public override void InitialElement()
	{

		// Puts the text to the initialized position
		movement = new ActionMovement()
			.AddAction(new StillPath(position),
			new ConstantProgression(waitTime + zoomTime));

		// Sets the scale of text to 0, then zooms text to full size
		zoomTransition = new ActionZoom()
			.AddAction(
			new Vector2(0, 0),
			new Vector2(0, 0),
			new ConstantProgression(waitTime))

			.AddAction(
			new Vector2(0, 0),
			new Vector2(1, 1),
			new EaseInOut(zoomTime / 2f, zoomTime));
	}

}
