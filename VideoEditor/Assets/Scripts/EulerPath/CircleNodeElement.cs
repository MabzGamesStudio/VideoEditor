using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleNodeElement : Element
{

	public float popTime;

	public float waitTime;

	public override void InitialElement()
	{
		zoomTransition = new ActionZoom()
			.AddAction(new Vector2(0, 0), new Vector2(0, 0), new ConstantProgression(waitTime))
			.AddAction(new Vector2(0, 0), new Vector2(1, 1), new EaseInOut(popTime / 2, popTime));
		movement = new ActionMovement()
			.AddAction(
			new StillPath(
				new Vector2(transform.localPosition.x,
				transform.localPosition.y)),
			new ConstantProgression(waitTime + popTime));
	}
}
