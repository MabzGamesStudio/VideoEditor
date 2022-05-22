using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Movement
{
	public MovementType GetMovementType();

	public float GetMovementPercent(float time);

	public float GetMovementTime();
}

public enum MovementType
{
	Constant,
	EaseIn,
	EaseOut,
	EaseInOut
}

public class ConstantMovement : Movement
{

	float moveTime;

	public ConstantMovement(float moveTime)
	{
		this.moveTime = moveTime;
	}

	public MovementType GetMovementType()
	{
		return MovementType.Constant;
	}

	public float GetMovementPercent(float time)
	{
		return time / moveTime;
	}

	public float GetMovementTime()
	{
		return moveTime;
	}
}