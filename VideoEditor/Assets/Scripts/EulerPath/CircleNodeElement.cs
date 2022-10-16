using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Element node pops in after waiting
/// </summary>
public class CircleNodeElement : Element
{

	/// <summary>
	/// Time the node takes to go from no size to full size
	/// </summary>
	public float popTime;

	/// <summary>
	/// Time the node spends waiting before popping
	/// </summary>
	public float waitTime;

	/// <summary>
	/// Initializes the node with a constant position movement action and
	/// a zoom action from no size to full size
	/// </summary>
	public override void InitialElement()
	{

		// A wait action, then a zoom in action with ease
		zoomTransition = new ActionZoom()
			.AddAction(
			new Vector2(0, 0),
			new Vector2(0, 0),
			new ConstantProgression(waitTime))
			.AddAction(
			new Vector2(0, 0),
			new Vector2(1, 1),
			new EaseInOut(popTime / 2, popTime));

		// Constrains the movement to be still
		movement = new ActionMovement()
			.AddAction(
			new StillPath(
				new Vector2(transform.localPosition.x,
				transform.localPosition.y)),
			new ConstantProgression(waitTime + popTime));
	}

}