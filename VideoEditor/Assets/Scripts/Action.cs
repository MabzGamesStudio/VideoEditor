using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{

	List<Path> paths;
	List<Movement> movements;
	List<float> movementTimes;

	float totalMoveTime;

	public Action()
	{
		totalMoveTime = 0;
		paths = new List<Path>();
		movements = new List<Movement>();
		movementTimes = new List<float>();
	}

	public Action(Path path, Movement movement)
	{
		totalMoveTime = 0;
		paths = new List<Path>();
		paths.Add(path);
		movements = new List<Movement>();
		movements.Add(movement);
		movementTimes = new List<float>();
		movementTimes.Add(movement.GetMovementTime());
	}

	public Action AddAction(Path path, Movement movement)
	{
		totalMoveTime += movement.GetMovementTime();
		paths.Add(path);
		movements.Add(movement);
		movementTimes.Add(movement.GetMovementTime());
		return this;
	}

	public bool ActionComplete(float time)
	{
		return totalMoveTime <= time;
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
				float movePercent = movements[i].GetMovementPercent(moveTime);
				return paths[i].GetPosition(movePercent);
			}
			moveTime--;
		}

		return new Vector2(0, 0);
	}
}
