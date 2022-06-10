using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Path
{
	public PathType GetMovementType();
	public Vector2 GetPosition(float percent);
}

public enum PathType
{
	Still,
	Linear,
	ParametricFunction,
	Function
}

public class StillPath : Path
{

	private Vector2 position;

	public StillPath(Vector2 position)
	{
		this.position = position;
	}

	public PathType GetMovementType()
	{
		return PathType.Still;
	}

	public Vector2 GetPosition(float percent)
	{
		return position;
	}
}

public class LinearPath : Path
{
	Vector2 startPosition;
	Vector2 endPosition;

	public LinearPath(Vector2 startPosition, Vector2 endPosition)
	{
		this.startPosition = startPosition;
		this.endPosition = endPosition;
	}

	public PathType GetMovementType()
	{
		return PathType.Linear;
	}

	public Vector2 GetPosition(float percent)
	{

		float xPosition = startPosition.x * (1 - percent) + endPosition.x * percent;
		float yPosition = startPosition.y * (1 - percent) + endPosition.y * percent;
		return new Vector2(xPosition, yPosition);
	}
}