using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgression
{
	public ProgressionType GetProgressionType();

	public float GetProgressionPercent(float time);

	public float GetProgressionTime();
}

public enum ProgressionType
{
	Constant,
	EaseIn,
	EaseOut,
	EaseInOut
}

public class ConstantProgression : IProgression
{

	float moveTime;

	public ConstantProgression(float moveTime)
	{
		this.moveTime = moveTime;
	}

	public ProgressionType GetProgressionType()
	{
		return ProgressionType.Constant;
	}

	public float GetProgressionPercent(float time)
	{
		return time / moveTime;
	}

	public float GetProgressionTime()
	{
		return moveTime;
	}
}