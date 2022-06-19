using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a path through x and y parametric functions as a function of t
/// </summary>
public class ParametricFunction : Path
{

	/// <summary>
	/// x function parameter
	/// </summary>
	private Function xFunction;

	/// <summary>
	/// y function parameter
	/// </summary>
	private Function yFunction;

	/// <summary>
	/// t value to start at
	/// </summary>
	private float tStart;

	/// <summary>
	/// t value to end at
	/// </summary>
	private float tEnd;

	/// <summary>
	/// Uses a parametric function to create a path
	/// </summary>
	/// <param name="xFunction">x function parameter</param>
	/// <param name="yFunction">y function parameter</param>
	/// <param name="tRange">Start and end t values</param>
	public ParametricFunction(Function xFunction, Function yFunction,
		Vector2 tRange)
	{
		this.xFunction = xFunction;
		this.yFunction = yFunction;
		tStart = tRange.x;
		tEnd = tRange.y;
	}

	/// <summary>
	/// Gets the ParametricFunction PathType
	/// </summary>
	/// <returns>ParametricFunction PathType enum</returns>
	PathType Path.GetMovementType()
	{
		return PathType.ParametricFunction;
	}

	/// <summary>
	/// Gets the position based on the parametric function and given percent
	/// from 0 to 1
	/// </summary>
	/// <param name="percent">Progress through the path</param>
	/// <returns>Position based on the parametric funciton</returns>
	Vector2 Path.GetPosition(float percent)
	{
		float t = tStart * (1 - percent) + tEnd * percent;
		Vector2 position;
		position.x = xFunction.GetY(t);
		position.y = yFunction.GetY(t);
		return position;
	}

}
