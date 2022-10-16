using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Text fades in and rises to position
/// </summary>
public class TextFadeElement : Element
{

	/// <summary>
	/// Time for element to fade into existence
	/// </summary>
	public float fadeTime;

	/// <summary>
	/// Wait time before fading into existence
	/// </summary>
	public float waitTime;

	/// <summary>
	/// Distance the text shifts up while fading in
	/// </summary>
	public float shiftAmount;

	/// <summary>
	/// Color the text fades into
	/// </summary>
	public Color textColor;

	/// <summary>
	/// Default transparent color
	/// </summary>
	private Color transparent = new Color(0, 0, 0, 0);

	/// <summary>
	/// Initializes element with upwards movement action and
	/// color fade from transparency
	/// </summary>
	public override void InitialElement()
	{

		// Sets absolute position to origin relative to the canvas
		transform.position = new Vector3(0, 0, 0);

		// Text waits then moves up
		movement = new ActionMovement()
			.AddAction(new StillPath(
				new Vector2(0, -shiftAmount)),
				new ConstantProgression(waitTime))
			.AddAction(new LinearPath(
				new Vector2(0, -shiftAmount),
				new Vector2(0, 0)),
				new EaseInOut(fadeTime / 2, fadeTime));

		// Text waits then fades into color
		colorTransition = new ActionColor()
			.AddAction(
			transparent,
			waitTime + fadeTime / 2)
			.AddAction(
			transparent,
			textColor,
			new ConstantProgression(fadeTime / 2));
	}

}