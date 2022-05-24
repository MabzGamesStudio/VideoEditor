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

	public bool centerZoom;

	public override void InitialElement()
	{

		movement = new ActionMovement()
			.AddAction(new StillPath(startPosition), new ConstantProgression(waitStart))
			.AddAction(new LinearPath(startPosition, endPosition), new ConstantProgression(moveTime))
			.AddAction(new StillPath(endPosition), new ConstantProgression(waitEnd));

		colorTransition = new ActionColor()
			.AddAction(colorStart, colorWait)
			.AddAction(colorStart, colorEnd, new ConstantProgression(colorTransitionTime));

		Vector2 zoomPivot;
		if (centerZoom)
		{
			zoomPivot = new Vector2(0, 0);
		}
		else
		{
			zoomPivot = new Vector2(-.5f, -.5f);
		}

		zoomTransition = new ActionZoom()
			.AddAction(new Vector2(1, 1),
			new Vector2(2, 2),
			new ConstantProgression(3),
			new ConstantProgression(3),
			zoomPivot);

	}

}
