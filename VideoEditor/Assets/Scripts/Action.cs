using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
	public bool ActionComplete(float time);

	public float TotalActionTime();

}

public class ActionMovement : IAction
{

	List<Path> paths;
	List<IProgression> movements;
	List<float> movementTimes;

	float totalMoveTime;

	public ActionMovement()
	{
		totalMoveTime = 0;
		paths = new List<Path>();
		movements = new List<IProgression>();
		movementTimes = new List<float>();
	}

	public ActionMovement(Path path, IProgression movement)
	{
		totalMoveTime = 0;
		paths = new List<Path>();
		paths.Add(path);
		movements = new List<IProgression>();
		movements.Add(movement);
		movementTimes = new List<float>();
		movementTimes.Add(movement.GetProgressionTime());
	}

	public ActionMovement AddAction(Path path, IProgression movement)
	{
		totalMoveTime += movement.GetProgressionTime();
		paths.Add(path);
		movements.Add(movement);
		movementTimes.Add(movement.GetProgressionTime());
		return this;
	}

	public bool ActionComplete(float time)
	{
		return totalMoveTime <= time;
	}

	public float TotalActionTime()
	{
		return totalMoveTime;
	}

	public Vector2 GetElementPosition(float time)
	{
		float moveTime = time;

		if (moveTime < 0)
		{
			return paths[0].GetPosition(0f);
		}
		if (moveTime > totalMoveTime)
		{
			return paths[paths.Count - 1].GetPosition(1f);
		}

		for (int i = 0; i < movementTimes.Count; i++)
		{
			if (movementTimes[i] > moveTime)
			{
				float movePercent = movements[i].GetProgressionPercent(moveTime);
				return paths[i].GetPosition(movePercent);
			}
			moveTime -= movementTimes[i];
		}

		return new Vector2(0, 0);
	}
}

public class ActionColor : IAction
{
	List<IProgression> progressions;
	List<float> transitionTimes;

	List<Color> startColors;
	List<Color> endColors;

	float totalMoveTime;

	public ActionColor()
	{
		totalMoveTime = 0;
		progressions = new List<IProgression>();
		transitionTimes = new List<float>();
		startColors = new List<Color>();
		endColors = new List<Color>();
	}

	public ActionColor(Color color, float time)
	{
		totalMoveTime = 0;
		progressions = new List<IProgression>();
		progressions.Add(new ConstantProgression(time));
		transitionTimes = new List<float>();
		transitionTimes.Add(time);
		startColors.Add(color);
		startColors.Add(color);
	}

	public ActionColor(Color startColor, Color endColor, IProgression progression)
	{
		totalMoveTime = 0;
		progressions = new List<IProgression>();
		progressions.Add(progression);
		transitionTimes = new List<float>();
		transitionTimes.Add(progression.GetProgressionTime());
		startColors.Add(startColor);
		startColors.Add(endColor);
	}

	public ActionColor AddAction(Color startColor, Color endColor, IProgression progression)
	{
		totalMoveTime += progression.GetProgressionTime();
		progressions.Add(progression);
		transitionTimes.Add(progression.GetProgressionTime());
		startColors.Add(startColor);
		endColors.Add(endColor);
		return this;
	}

	public ActionColor AddAction(Color color, float time)
	{
		totalMoveTime += time;
		progressions.Add(new ConstantProgression(time));
		transitionTimes.Add(time);
		startColors.Add(color);
		endColors.Add(color);
		return this;
	}

	public bool ActionComplete(float time)
	{
		return totalMoveTime <= time;
	}

	public float TotalActionTime()
	{
		return totalMoveTime;
	}

	public Color GetElementColor(float time)
	{
		float moveTime = time;

		if (moveTime < 0)
		{
			return startColors[0];
		}
		if (moveTime > totalMoveTime)
		{
			return endColors[endColors.Count - 1];
		}

		for (int i = 0; i < transitionTimes.Count; i++)
		{
			if (transitionTimes[i] > moveTime)
			{
				float movePercent = progressions[i].GetProgressionPercent(moveTime);

				float red = startColors[i].r * (1 - movePercent) + endColors[i].r * movePercent;
				float green = startColors[i].g * (1 - movePercent) + endColors[i].g * movePercent;
				float blue = startColors[i].b * (1 - movePercent) + endColors[i].b * movePercent;
				float alpha = startColors[i].a * (1 - movePercent) + endColors[i].a * movePercent;

				return new Color(red, green, blue, alpha);
			}
			moveTime -= transitionTimes[i];
		}

		return Color.black;
	}
}
