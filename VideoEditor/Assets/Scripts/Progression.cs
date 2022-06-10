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
	EaseInOut,
	RubberBand
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

public class EaseInOut : IProgression
{

	float moveTime;

	float easeTime;

	float Cofactor
	{
		get
		{
			if (_cofactor == 0f)
			{
				_cofactor = 1f / (2f * easeTime * moveTime - 2f * Mathf.Pow(easeTime, 2));
			}
			return _cofactor;
		}
	}
	private float _cofactor;

	public EaseInOut(float easeTime, float moveTime)
	{
		this.moveTime = moveTime;
		this.easeTime = easeTime;
		this.easeTime = Mathf.Min(easeTime, moveTime / 2f);
	}

	public ProgressionType GetProgressionType()
	{
		return ProgressionType.EaseInOut;
	}

	public float GetProgressionPercent(float time)
	{

		// Magic ease math
		if (time <= easeTime)
		{
			return Cofactor * Mathf.Pow(time, 2);
		}
		else if (time <= moveTime - easeTime)
		{
			return Cofactor * (2f * easeTime * time - Mathf.Pow(easeTime, 2));
		}
		else
		{
			return Cofactor * (2f * easeTime * moveTime - 2f * Mathf.Pow(easeTime, 2) - Mathf.Pow(time - moveTime, 2));
		}
	}

	public float GetProgressionTime()
	{
		return moveTime;
	}
}

public class Rubberband : IProgression
{

	float moveTime;

	float rubber;

	float Cofactor
	{
		get
		{
			if (_cofactor == 0f)
			{
				_cofactor = 1f / (2f * rubber * moveTime - 4f * Mathf.Pow(rubber, 2));
			}
			return _cofactor;
		}
	}
	private float _cofactor;

	public Rubberband(float rubberTime, float moveTime)
	{
		this.moveTime = moveTime;
		this.rubber = rubberTime / 2;
		this.rubber = Mathf.Min(rubber, moveTime / 3f);
	}

	public ProgressionType GetProgressionType()
	{
		return ProgressionType.EaseInOut;
	}

	public float GetProgressionPercent(float time)
	{

		// Magic rubber math
		if (time <= moveTime - 2f * rubber)
		{
			return Cofactor * 2f * rubber * time;
		}
		else
		{
			return Cofactor * (-Mathf.Pow(time - moveTime + rubber, 2) + 2f * rubber * moveTime - 3f * Mathf.Pow(rubber, 2));
		}
	}

	public float GetProgressionTime()
	{
		return moveTime;
	}
}