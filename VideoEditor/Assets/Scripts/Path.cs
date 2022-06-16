using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface of path through 2d space
/// </summary>
public interface Path
{

	/// <summary>
	/// Gets the PathType enum
	/// </summary>
	/// <returns>PathType enum</returns>
	public PathType GetMovementType();

	/// <summary>
	/// Gets the position at a certain percent of the progression
	/// </summary>
	/// <param name="percent">Progression percent from 0 to 1</param>
	/// <returns>Position at the given percent progression</returns>
	public Vector2 GetPosition(float percent);
}

/// <summary>
/// Path type (ie: Still, Linear, Function)
/// </summary>
public enum PathType
{
	Still,
	Linear,
	ParametricFunction,
	Function
}

/// <summary>
/// Still path
/// </summary>
public class StillPath : Path
{

	/// <summary>
	/// Position
	/// </summary>
	private Vector2 position;

	/// <summary>
	/// Creates path with a single point position
	/// </summary>
	/// <param name="position">Position</param>
	public StillPath(Vector2 position)
	{
		this.position = position;
	}

	/// <summary>
	/// Gets the Still PathType enum
	/// </summary>
	/// <returns>Still PathType enum</returns>
	public PathType GetMovementType()
	{
		return PathType.Still;
	}

	/// <summary>
	/// Gets the position given the progress
	/// </summary>
	/// <param name="percent">Progress percent from 0 to 1</param>
	/// <returns>The position</returns>
	public Vector2 GetPosition(float percent)
	{
		return position;
	}
}

/// <summary>
/// Linear path
/// </summary>
public class LinearPath : Path
{

	/// <summary>
	/// The start position of the path
	/// </summary>
	Vector2 startPosition;

	/// <summary>
	/// The end position of the path
	/// </summary>
	Vector2 endPosition;

	/// <summary>
	/// Creates a linear path that goes from the start position to the end
	/// position
	/// </summary>
	/// <param name="startPosition">Position at 0</param>
	/// <param name="endPosition">Position at 1</param>
	public LinearPath(Vector2 startPosition, Vector2 endPosition)
	{
		this.startPosition = startPosition;
		this.endPosition = endPosition;
	}

	/// <summary>
	/// Gets the LinearPath PathType enum
	/// </summary>
	/// <returns>LinearPath PathType enum</returns>
	public PathType GetMovementType()
	{
		return PathType.Linear;
	}

	/// <summary>
	/// Gets the position on the linear path given the progression from 0 to 1
	/// </summary>
	/// <param name="percent">The progression through the path from 0 to
	/// 1</param>
	/// <returns>The position on the path</returns>
	public Vector2 GetPosition(float percent)
	{

		// The individual x and y positions are calculated as linear progression
		float xPosition = startPosition.x * (1 - percent) + endPosition.x *
			percent;
		float yPosition = startPosition.y * (1 - percent) + endPosition.y *
			percent;

		// The x and y values are created to make a position
		return new Vector2(xPosition, yPosition);
	}
}