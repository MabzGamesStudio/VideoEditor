using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanElement : Element
{

	public Vector2 startPosition;

	public Vector2 endPosition;

	public float zoomPercent;

	public float moveTime;

	public override void InitialElement()
	{

		movement = new ActionMovement()
			.AddAction(new LinearPath(-startPosition, -endPosition), new ConstantProgression(moveTime));

		zoomTransition = new ActionZoom()
			.AddAction(new Vector2(1, 1), new Vector2(zoomPercent, zoomPercent), new ConstantProgression(moveTime));
	}
}
