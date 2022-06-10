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
			.AddAction(new LinearPath(startPosition, endPosition), new Rubberband(moveTime / 4f, moveTime));


		//colorTransition = new ActionColor()
		//	.AddAction(colorStart, colorWait)
		//	.AddAction(colorStart, colorEnd, new ConstantProgression(colorTransitionTime));


		//zoomTransition = new ActionZoom()
		//	.AddAction(new Vector2(1, 1),
		//	new Vector2(1, 2),
		//	new ConstantProgression(3),
		//	new ConstantProgression(3),
		//	new Vector2(0f, 0f));

		//rotateTransition = new ActionRotate();
		//.AddAction(0, 180, new ConstantProgression(1), new Vector2(0, 1f));
	}

}
