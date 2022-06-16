using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to have a variable progression from 0 to 1
/// </summary>
public interface IProgression
{

	/// <summary>
	/// Gets the enum type of progression (ie: constant, ease, rubber band)
	/// </summary>
	/// <returns>Enum type of progression</returns>
	public ProgressionType GetProgressionType();

	/// <summary>
	/// Gets the progression percentage from 0 to 1 based on the time
	/// </summary>
	/// <param name="time">The time through the progression</param>
	/// <returns>Progression percentage (typical range from 0 to 1 but can
	/// extend range) (starts at 0 and ends at 1)</returns>
	public float GetProgressionPercent(float time);

	/// <summary>
	/// Gets the total time for the progression
	/// </summary>
	/// <returns>Total time for the progression</returns>
	public float GetProgressionTime();
}

/// <summary>
/// The type of progression (ie: constant. ease, rubber band)
/// </summary>
public enum ProgressionType
{
	Constant,
	EaseInOut,
	RubberBand
}

/// <summary>
/// Constant progression has linear change from 0 to 1
/// </summary>
public class ConstantProgression : IProgression
{

	/// <summary>
	/// The total time to progress from 0 to 1
	/// </summary>
	float moveTime;

	/// <summary>
	/// Creates constant progression from 0 to 1
	/// </summary>
	/// <param name="moveTime">The total time to progress from 0 to 1</param>
	public ConstantProgression(float moveTime)
	{
		this.moveTime = moveTime;
	}

	/// <summary>
	/// Gets the constant progression enum type
	/// </summary>
	/// <returns>Constant progression enum</returns>
	public ProgressionType GetProgressionType()
	{
		return ProgressionType.Constant;
	}

	/// <summary>
	/// Returns progression from 0 to 1 based on given time
	/// </summary>
	/// <param name="time">Time progressed</param>
	/// <returns>Constant progress from 0 to 1</returns>
	public float GetProgressionPercent(float time)
	{
		return time / moveTime;
	}

	/// <summary>
	/// Gets the total progress time
	/// </summary>
	/// <returns>Total move time</returns>
	public float GetProgressionTime()
	{
		return moveTime;
	}
}

/// <summary>
/// Semi-constant progression from 0 to 1, with easing at the start and end
/// </summary>
public class EaseInOut : IProgression
{

	/// <summary>
	/// Total time to move
	/// </summary>
	float moveTime;

	/// <summary>
	/// Time for easing in and easing out
	/// </summary>
	float easeTime;

	/// <summary>
	/// Cofactor that allows progression to start at 0 and end at 1
	/// </summary>
	float Cofactor
	{
		get
		{
			if (_cofactor == 0f)
			{

				// Math magic
				_cofactor = 1f / (2f * easeTime * moveTime - 2f *
					Mathf.Pow(easeTime, 2));
			}
			return _cofactor;
		}
	}
	private float _cofactor;

	/// <summary>
	/// Creates easing progression from 0 to 1
	/// </summary>
	/// <param name="easeTime">Time for easing in and out</param>
	/// <param name="moveTime">Total time to move</param>
	public EaseInOut(float easeTime, float moveTime)
	{
		this.moveTime = moveTime;
		this.easeTime = easeTime;
		this.easeTime = Mathf.Min(easeTime, moveTime / 2f);
	}

	/// <summary>
	/// Gets the EaseInOut progression enum type
	/// </summary>
	/// <returns>EaseInOut progression enum</returns>
	public ProgressionType GetProgressionType()
	{
		return ProgressionType.EaseInOut;
	}

	/// <summary>
	/// Returns progression from 0 to 1 based on given time, with easing
	/// </summary>
	/// <param name="time">Time progression</param>
	/// <returns>Semi-constant progress from 0 to 1 with easing at start and
	/// end</returns>
	public float GetProgressionPercent(float time)
	{

		// Magic ease math
		// At start of ease ramp up speed
		if (time <= easeTime)
		{
			return Cofactor * Mathf.Pow(time, 2);
		}

		// Between easing move constant
		else if (time <= moveTime - easeTime)
		{
			return Cofactor * (2f * easeTime * time - Mathf.Pow(easeTime, 2));
		}

		// At end of ease ramp down speed
		else
		{
			return Cofactor * (2f * easeTime * moveTime - 2f *
				Mathf.Pow(easeTime, 2) - Mathf.Pow(time - moveTime, 2));
		}
	}

	/// <summary>
	/// Gets the total progress time
	/// </summary>
	/// <returns>Total move time</returns>
	public float GetProgressionTime()
	{
		return moveTime;
	}
}

/// <summary>
/// Semi-constant progression from 0 to 1, with overshot at end that snaps back
/// to end
/// </summary>
public class Rubberband : IProgression
{

	/// <summary>
	/// Total move time
	/// </summary>
	float moveTime;

	/// <summary>
	/// Time that overshoots and rubbers back
	/// </summary>
	float rubber;

	/// <summary>
	/// Cofactor that allows progression to start at 0 and end at 1
	/// </summary>
	float Cofactor
	{
		get
		{
			if (_cofactor == 0f)
			{
				_cofactor = 1f / (2f * rubber * moveTime - 4f *
					Mathf.Pow(rubber, 2));
			}
			return _cofactor;
		}
	}
	private float _cofactor;

	/// <summary>
	/// Creates rubber progression from 0 to 1
	/// </summary>
	/// <param name="rubberTime">Time that overshoots end</param>
	/// <param name="moveTime">Total progression time</param>
	public Rubberband(float rubberTime, float moveTime)
	{
		this.moveTime = moveTime;
		this.rubber = rubberTime / 2;
		this.rubber = Mathf.Min(rubber, moveTime / 3f);
	}

	/// <summary>
	/// Gets the RubberBand progression enum type
	/// </summary>
	/// <returns>RubberBand progression enum</returns>
	public ProgressionType GetProgressionType()
	{
		return ProgressionType.EaseInOut;
	}

	/// <summary>
	/// Returns progression from 0 to 1 based on given time, with rubber banding
	/// (overshoots past 1 then returns)
	/// </summary>
	/// <param name="time">Time progression</param>
	/// <returns>Semi-constant progress from 0 to 1 with overshoot at
	/// end</returns>
	public float GetProgressionPercent(float time)
	{

		// Magic rubber math
		// Moving constant at start
		if (time <= moveTime - 2f * rubber)
		{
			return Cofactor * 2f * rubber * time;
		}

		// Overshoots endpoint and rubber bands back to the end
		else
		{
			return Cofactor * (-Mathf.Pow(time - moveTime + rubber, 2) + 2f *
				rubber * moveTime - 3f * Mathf.Pow(rubber, 2));
		}
	}

	/// <summary>
	/// Gets the total progress time
	/// </summary>
	/// <returns>Total move time</returns>
	public float GetProgressionTime()
	{
		return moveTime;
	}
}