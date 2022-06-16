using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript : Element
{

	/// <summary>
	/// Start position of the circle
	/// </summary>
	public Vector2 startPosition;

	/// <summary>
	/// End position of the circle
	/// </summary>
	public Vector2 endPosition;

	/// <summary>
	/// Time to move circle
	/// </summary>
	public float moveTime;

	/// <summary>
	/// Wait time before color changes
	/// </summary>
	public float colorWait;

	/// <summary>
	/// Start color
	/// </summary>
	public Color colorStart;

	/// <summary>
	/// Ending color
	/// </summary>
	public Color colorEnd;

	/// <summary>
	/// Time to change color
	/// </summary>
	public float colorTransitionTime;

	/// <summary>
	/// Circle actions are set
	/// </summary>
	public override void InitialElement()
	{

		// Moves from left to right
		movement = new ActionMovement()
			.AddAction(new LinearPath(startPosition, endPosition), new EaseInOut(moveTime / 4f, moveTime));

		// Waits, then changes color
		colorTransition = new ActionColor()
			.AddAction(colorStart, colorWait)
			.AddAction(colorStart, colorEnd, new ConstantProgression(colorTransitionTime));

		// Circle scales twice as big
		zoomTransition = new ActionZoom()
			.AddAction(new Vector2(1, 1),
			new Vector2(2, 2),
			new ConstantProgression(2),
			new ConstantProgression(3),
			new Vector2(.5f, .5f));

		// Circle rotates half turn
		rotateTransition = new ActionRotate()
			.AddAction(0, 180, new ConstantProgression(1), new Vector2(0, 1f));
	}

}
