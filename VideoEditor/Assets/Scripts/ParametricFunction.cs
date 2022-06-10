using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametricFunction : Path
{

	private Function xFunction;
	private Function yFunction;

	private float tStart;

	private float tEnd;

	public ParametricFunction(Function xFunction, Function yFunction, Vector2 tRange)
	{
		this.xFunction = xFunction;
		this.yFunction = yFunction;
		tStart = tRange.x;
		tEnd = tRange.y;
	}

	PathType Path.GetMovementType()
	{
		return PathType.ParametricFunction;
	}

	Vector2 Path.GetPosition(float percent)
	{
		float t = tStart * (1 - percent) + tEnd * percent;
		Vector2 position;
		position.x = xFunction.GetY(t);
		position.y = yFunction.GetY(t);
		return position;
	}
}
