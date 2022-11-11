using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Text fades in and rises to position
/// </summary>
public class TextFadeElement : Element
{

	/// <summary>
	/// Wait time before fading into existence
	/// </summary>
	public float waitTime;

	/// <summary>
	/// Time for element to fade into existence
	/// </summary>
	public float fadeInTime;

	/// <summary>
	/// Time between fade in and fade out
	/// </summary>
	public float intermediateTime;

	/// <summary>
	/// Time for element to fade out of existence, if -1 then no fade out
	/// </summary>
	public float fadeOutTime;

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
				new EaseInOut(fadeInTime / 2, fadeInTime));

		// Wait then move back if desired
		if (fadeOutTime != -1)
		{
			movement
				.AddAction(new StillPath(
				new Vector2(0, 0)),
				new ConstantProgression(intermediateTime))
				.AddAction(new LinearPath(
				new Vector2(0, 0),
				new Vector2(0, -shiftAmount)),
				new EaseInOut(fadeOutTime / 2, fadeOutTime));
		}

		// Text waits then fades into color
		colorTransition = new ActionColor()
		.AddAction(
		transparent,
		waitTime + fadeInTime / 2)
		.AddAction(
		transparent,
		textColor,
		new ConstantProgression(fadeInTime / 2));

		// Wait then change back to trasparent if desired
		if (fadeOutTime != -1)
		{
			colorTransition
				.AddAction(
				textColor,
				intermediateTime)
				.AddAction(
				textColor,
				transparent,
				new ConstantProgression(fadeOutTime / 2));
		}
	}

}