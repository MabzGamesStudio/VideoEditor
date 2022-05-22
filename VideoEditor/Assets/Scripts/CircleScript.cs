using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript : Element
{

	public Vector2 startPosition;

	public Vector2 endPosition;

	public float waitStart;

	public float moveTime;

	public float waitEnd;

	public float colorWait;

	public Color colorStart;

	public Color colorEnd;

	public float colorTransitionTime;

	public override void InitialElement()
	{

		movement = new ActionMovement()
			.AddAction(new StillPath(startPosition), new ConstantProgression(waitStart))
			.AddAction(new LinearPath(startPosition, endPosition), new ConstantProgression(moveTime))
			.AddAction(new StillPath(endPosition), new ConstantProgression(waitEnd));

		colorTransition = new ActionColor()
			.AddAction(colorStart, colorWait)
			.AddAction(colorStart, colorEnd, new ConstantProgression(colorTransitionTime));
	}

}
