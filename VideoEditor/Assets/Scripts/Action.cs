using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface that allows a physical action of an element
/// </summary>
public interface IAction
{

	/// <summary>
	/// Tells whether the action has been completed at the given time
	/// </summary>
	/// <param name="time">Time to test whether action has been
	/// completed</param>
	/// <returns>Whether the action has been completed at given time</returns>
	public bool ActionComplete(float time);

	/// <summary>
	/// Gets the total time to complete the action
	/// </summary>
	/// <returns>Total time to complete action</returns>
	public float TotalActionTime();

}

/// <summary>
/// Action that controls the movement of the element
/// </summary>
public class ActionMovement : IAction
{

	/// <summary>
	/// The ordered paths the element moves throughout
	/// </summary>
	List<Path> paths;

	/// <summary>
	/// The progress in which the element moves throughout the paths
	/// </summary>
	List<IProgression> movements;

	/// <summary>
	/// The completion times for each of the movements
	/// </summary>
	List<float> movementTimes;

	/// <summary>
	/// The total time of all movements
	/// </summary>
	float totalMoveTime;

	/// <summary>
	/// Initializes action movement
	/// </summary>
	public ActionMovement()
	{
		totalMoveTime = 0;
		paths = new List<Path>();
		movements = new List<IProgression>();
		movementTimes = new List<float>();
	}

	/// <summary>
	/// Initializes action movement with path and progression input
	/// </summary>
	/// <param name="path">Path for element to follow</param>
	/// <param name="movement">Progression through path</param>
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

	/// <summary>
	/// Adds action movement with path and progression input
	/// </summary>
	/// <param name="path">Path for element to follow</param>
	/// <param name="movement">Progression through path</param>
	/// <returns>Same action with appended input action</returns>
	public ActionMovement AddAction(Path path, IProgression movement)
	{
		totalMoveTime += movement.GetProgressionTime();
		paths.Add(path);
		movements.Add(movement);
		movementTimes.Add(movement.GetProgressionTime());
		return this;
	}

	/// <summary>
	/// Whether the entire action has been completed at the given time
	/// </summary>
	/// <param name="time">Time to test if action is complete</param>
	/// <returns>Whether the entire action has been completed at the given
	/// time</returns>
	public bool ActionComplete(float time)
	{
		return totalMoveTime <= time;
	}

	/// <summary>
	/// Gets the total time to complete the action
	/// </summary>
	/// <returns>Total time for action to complete</returns>
	public float TotalActionTime()
	{
		return totalMoveTime;
	}

	/// <summary>
	/// Gets the position of the element at the spcified time
	/// </summary>
	/// <param name="time">Time through action</param>
	/// <returns>Position of the element at given time</returns>
	public Vector2 GetElementPosition(float time)
	{

		// Keeps track of time through the individual actions
		float moveTime = time;

		// Edge cases
		if (moveTime <= 0)
		{
			return paths[0].GetPosition(0f);
		}
		if (moveTime >= totalMoveTime)
		{
			return paths[paths.Count - 1].GetPosition(1f);
		}

		// Loops through each individual action
		for (int i = 0; i < movementTimes.Count; i++)
		{

			// If time is within individual action, then get position from time
			// through the individual action
			if (movementTimes[i] > moveTime)
			{
				float movePercent =
					movements[i].GetProgressionPercent(moveTime);
				return paths[i].GetPosition(movePercent);
			}
			moveTime -= movementTimes[i];
		}

		// Shouldn't be reached, default value for error case
		return new Vector2(0, 0);
	}

}

/// <summary>
/// Action that controls the color of the element
/// </summary>
public class ActionColor : IAction
{

	/// <summary>
	/// The progression of color change for all color transitions
	/// </summary>
	List<IProgression> progressions;

	/// <summary>
	/// The completion times for each of the color transitions
	/// </summary>
	List<float> transitionTimes;

	/// <summary>
	/// The starting color at each transition phase
	/// </summary>
	List<Color> startColors;

	/// <summary>
	/// The ending color at each transition phase
	/// </summary>
	List<Color> endColors;

	/// <summary>
	/// The total time of all color transitions
	/// </summary>
	float totalMoveTime;

	/// <summary>
	/// Initializes action color
	/// </summary>
	public ActionColor()
	{
		totalMoveTime = 0;
		progressions = new List<IProgression>();
		transitionTimes = new List<float>();
		startColors = new List<Color>();
		endColors = new List<Color>();
	}

	/// <summary>
	/// Initializes action color with solid color
	/// </summary>
	/// <param name="color">Element color</param>
	/// <param name="time">Time to sustain color</param>
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

	/// <summary>
	/// Initializes action color with color transition
	/// </summary>
	/// <param name="startColor">Start color</param>
	/// <param name="endColor">End color</param>
	/// <param name="progression">Progression of color transition</param>
	public ActionColor(Color startColor, Color endColor,
		IProgression progression)
	{
		totalMoveTime = 0;
		progressions = new List<IProgression>();
		progressions.Add(progression);
		transitionTimes = new List<float>();
		transitionTimes.Add(progression.GetProgressionTime());
		startColors.Add(startColor);
		startColors.Add(endColor);
	}

	/// <summary>
	/// Adds action color with color transition
	/// </summary>
	/// <param name="startColor">Start color</param>
	/// <param name="endColor">End color</param>
	/// <param name="progression">Progression of color transition</param>
	/// <returns>Same action with appended input action</returns>
	public ActionColor AddAction(Color startColor, Color endColor,
		IProgression progression)
	{
		totalMoveTime += progression.GetProgressionTime();
		progressions.Add(progression);
		transitionTimes.Add(progression.GetProgressionTime());
		startColors.Add(startColor);
		endColors.Add(endColor);
		return this;
	}

	/// <summary>
	/// Adds action color with color transition
	/// </summary>
	/// <param name="color">Element color</param>
	/// <param name="time">Time to sustain color</param>
	/// <returns>Same action with appended input action</returns>
	public ActionColor AddAction(Color color, float time)
	{
		totalMoveTime += time;
		progressions.Add(new ConstantProgression(time));
		transitionTimes.Add(time);
		startColors.Add(color);
		endColors.Add(color);
		return this;
	}

	/// <summary>
	/// Whether the entire action has been completed at the given time
	/// </summary>
	/// <param name="time">Time to test if action is complete</param>
	/// <returns>Whether the entire action has been completed at the given
	/// time</returns>
	public bool ActionComplete(float time)
	{
		return totalMoveTime <= time;
	}

	/// <summary>
	/// Gets the total time to complete the action
	/// </summary>
	/// <returns>Total time for action to complete</returns>
	public float TotalActionTime()
	{
		return totalMoveTime;
	}

	/// <summary>
	/// Gets the color of the element at the spcified time
	/// </summary>
	/// <param name="time">Time through action</param>
	/// <returns>Color of the element at the given time</returns>
	public Color GetElementColor(float time)
	{

		// Keeps track of time through the individual actions
		float moveTime = time;

		// Edge cases
		if (moveTime <= 0)
		{
			return startColors[0];
		}
		if (moveTime >= totalMoveTime)
		{
			return endColors[endColors.Count - 1];
		}

		// Loops through each individual action
		for (int i = 0; i < transitionTimes.Count; i++)
		{

			// If time is within individual action, then get color from time
			// through the individual action
			if (transitionTimes[i] > moveTime)
			{
				float movePercent =
					progressions[i].GetProgressionPercent(moveTime);

				// Linearly interpolate each of the color channels
				float red = startColors[i].r * (1 - movePercent) +
					endColors[i].r * movePercent;
				float green = startColors[i].g * (1 - movePercent) +
					endColors[i].g * movePercent;
				float blue = startColors[i].b * (1 - movePercent) +
					endColors[i].b * movePercent;
				float alpha = startColors[i].a * (1 - movePercent) +
					endColors[i].a * movePercent;
				return new Color(red, green, blue, alpha);
			}
			moveTime -= transitionTimes[i];
		}

		// Shouldn't be reached, default value for error case
		return Color.black;
	}

}

/// <summary>
/// Action that controls the scale of the element
/// </summary>
public class ActionZoom : IAction
{

	/// <summary>
	/// The progression of width for each zoom transition
	/// </summary>
	List<IProgression> widthProgressions;

	/// <summary>
	/// The progression of height for each zoom transition
	/// </summary>
	List<IProgression> heightProgressions;

	/// <summary>
	/// The completion times for each of the zoom transitions
	/// </summary>
	List<float> transitionTimes;

	/// <summary>
	/// The starting size of the element at the start of each phase
	/// </summary>
	List<Vector2> startSizes;

	/// <summary>
	/// The ending size of the element at the end of each phase
	/// </summary>
	List<Vector2> endSizes;

	/// <summary>
	/// The pivot positions to scale the element at for each phase
	/// </summary>
	List<Vector2> pivotPositions;

	/// <summary>
	/// The total time of all zoom transitions
	/// </summary>
	float totalMoveTime;

	/// <summary>
	/// Initializes action zoom
	/// </summary>
	public ActionZoom()
	{
		totalMoveTime = 0;
		widthProgressions = new List<IProgression>();
		heightProgressions = new List<IProgression>();
		transitionTimes = new List<float>();
		startSizes = new List<Vector2>();
		endSizes = new List<Vector2>();
		pivotPositions = new List<Vector2>();
	}

	/// <summary>
	/// Initializes action zoom with simple zoom inuput
	/// </summary>
	/// <param name="startSize">Start size</param>
	/// <param name="endSize">End size</param>
	/// <param name="progression">Progression of size zoom</param>
	public ActionZoom(Vector2 startSize, Vector2 endSize,
		IProgression progression)
	{
		totalMoveTime = 0;
		widthProgressions = new List<IProgression>();
		heightProgressions = new List<IProgression>();
		widthProgressions.Add(progression);
		heightProgressions.Add(progression);
		transitionTimes = new List<float>();
		transitionTimes.Add(progression.GetProgressionTime());
		startSizes.Add(startSize);
		endSizes.Add(endSize);
		pivotPositions.Add(new Vector2(0, 0));
	}

	/// <summary>
	/// Initializes action zoom with complex zoom inuput
	/// </summary>
	/// <param name="startSize">Start size</param>
	/// <param name="endSize">End size</param>
	/// <param name="widthProgression">Progression of width size zoom</param>
	/// <param name="heightProgression">Progression of height size zoom</param>
	/// <param name="pivotPosition">Position of pivot to zoom from</param>
	public ActionZoom(Vector2 startSize, Vector2 endSize,
		IProgression widthProgression, IProgression heightProgression,
		Vector2 pivotPosition)
	{
		totalMoveTime = 0;
		widthProgressions = new List<IProgression>();
		heightProgressions = new List<IProgression>();
		widthProgressions.Add(widthProgression);
		heightProgressions.Add(heightProgression);
		transitionTimes = new List<float>();
		transitionTimes.Add(Mathf.Max(widthProgression.GetProgressionTime(),
			heightProgression.GetProgressionTime()));
		startSizes.Add(startSize);
		endSizes.Add(endSize);
		pivotPositions.Add(pivotPosition);
	}

	/// <summary>
	/// Adds action zoom with simple zoom inuput
	/// </summary>
	/// <param name="startSize">Start size</param>
	/// <param name="endSize">End size</param>
	/// <param name="progression">Progression of size zoom</param>
	/// <returns>Same action with appended input action</returns>
	public ActionZoom AddAction(Vector2 startSize, Vector2 endSize,
		IProgression progression)
	{
		widthProgressions.Add(progression);
		heightProgressions.Add(progression);
		transitionTimes.Add(progression.GetProgressionTime());
		startSizes.Add(startSize);
		endSizes.Add(endSize);
		pivotPositions.Add(new Vector2(0, 0));
		totalMoveTime += transitionTimes[transitionTimes.Count - 1];
		return this;
	}

	/// <summary>
	/// Adds action zoom with complex zoom inuput
	/// </summary>
	/// <param name="startSize">Start size</param>
	/// <param name="endSize">End size</param>
	/// <param name="widthProgression">Progression of width size zoom</param>
	/// <param name="heightProgression">Progression of height size zoom</param>
	/// <param name="pivotPosition">Position of pivot to zoom from</param>
	/// <returns>Same action with appended input action</returns>
	public ActionZoom AddAction(Vector2 startSize, Vector2 endSize,
		IProgression widthProgression, IProgression heightProgression,
		Vector2 pivotPosition)
	{
		widthProgressions.Add(widthProgression);
		heightProgressions.Add(heightProgression);
		transitionTimes.Add(Mathf.Max(widthProgression.GetProgressionTime(),
			heightProgression.GetProgressionTime()));
		startSizes.Add(startSize);
		endSizes.Add(endSize);
		pivotPositions.Add(pivotPosition);
		totalMoveTime += transitionTimes[transitionTimes.Count - 1];
		return this;
	}

	/// <summary>
	/// Whether the entire action has been completed at the given time
	/// </summary>
	/// <param name="time">Time to test if action is complete</param>
	/// <returns>Whether the entire action has been completed at the given
	/// time</returns>
	public bool ActionComplete(float time)
	{
		return totalMoveTime <= time;
	}

	/// <summary>
	/// Gets the total time to complete the action
	/// </summary>
	/// <returns>Total time for action to complete</returns>
	public float TotalActionTime()
	{
		return totalMoveTime;
	}

	/// <summary>
	/// Gets the position of the pivot given the specified time
	/// </summary>
	/// <param name="time">Time through action</param>
	/// <returns>The position of the pivot at the given time</returns>
	public Vector2 GetPivotPosition(float time)
	{

		// Keeps track of time through the individual actions
		float pivotTime = time;

		// Edge cases
		if (pivotTime <= 0)
		{
			return pivotPositions[0];
		}
		if (pivotTime >= totalMoveTime)
		{
			return pivotPositions[pivotPositions.Count - 1];
		}

		// Loops through each individual action
		for (int i = 0; i < transitionTimes.Count; i++)
		{

			// If time is within individual action, then get the pivot position
			// for the individual action
			if (transitionTimes[i] > pivotTime)
			{
				return pivotPositions[i];
			}
			pivotTime -= transitionTimes[i];
		}

		// Shouldn't be reached, default value for error case
		return new Vector2(0, 0);
	}

	/// <summary>
	/// Gets the size of the element at the spcified time
	/// </summary>
	/// <param name="time">Time through action</param>
	/// <returns>Size of the element at the given time</returns>
	public Vector2 GetElementSize(float time)
	{

		// Keeps track of time through the individual actions
		float pivotTime = time;

		// Edge cases
		if (pivotTime <= 0)
		{
			return startSizes[0];
		}
		if (pivotTime >= totalMoveTime)
		{
			return endSizes[endSizes.Count - 1];
		}

		// Loops through each individual action
		for (int i = 0; i < transitionTimes.Count; i++)
		{

			// If time is within individual action, then get zoom from time
			// through the individual action
			if (transitionTimes[i] > pivotTime)
			{
				float widthPercent =
					widthProgressions[i].GetProgressionPercent(pivotTime);
				float heightPercent =
					heightProgressions[i].GetProgressionPercent(pivotTime);
				float width = startSizes[i].x * (1 - widthPercent) +
					endSizes[i].x * widthPercent;
				float height = startSizes[i].y * (1 - heightPercent) +
					endSizes[i].y * heightPercent;
				return new Vector2(width, height);
			}
			pivotTime -= transitionTimes[i];
		}

		// Shouldn't be reached, default value for error case
		return new Vector2(1, 1);
	}

}

/// <summary>
/// Action that controls the rotation of the element
/// </summary>
public class ActionRotate : IAction
{

	/// <summary>
	/// The progression of color change for all color transitions
	/// </summary>
	List<IProgression> progressions;

	/// <summary>
	/// The completion times for each of the rotate transitions
	/// </summary>
	List<float> transitionTimes;

	/// <summary>
	/// The start of the rotation angle for each rotation phase
	/// </summary>
	List<float> startRotations;

	/// <summary>
	/// The end of the rotation angle for each rotation phase
	/// </summary>
	List<float> endRotations;

	/// <summary>
	/// The positions for the element to rotate around for each phase
	/// </summary>
	List<Vector2> pivotPositions;

	/// <summary>
	/// The total time of all zoom transitions
	/// </summary>
	float totalMoveTime;

	/// <summary>
	/// Initializes action rotate
	/// </summary>
	public ActionRotate()
	{
		totalMoveTime = 0;
		progressions = new List<IProgression>();
		transitionTimes = new List<float>();
		startRotations = new List<float>();
		endRotations = new List<float>();
		pivotPositions = new List<Vector2>();
	}

	/// <summary>
	/// Initialize action rotate with simple input
	/// </summary>
	/// <param name="startRotation">The angle to start the rotation</param>
	/// <param name="endRotation">The angle to end the rotation</param>
	/// <param name="progression">Progression to transition rotation</param>
	public ActionRotate(float startRotation, float endRotation,
		IProgression progression)
	{
		totalMoveTime = 0;
		progressions = new List<IProgression>();
		progressions.Add(progression);
		transitionTimes = new List<float>();
		transitionTimes.Add(progression.GetProgressionTime());
		startRotations.Add(startRotation);
		endRotations.Add(endRotation);
		pivotPositions.Add(new Vector2(0, 0));
	}

	/// <summary>
	/// Initialize action rotate with complex input
	/// </summary>
	/// <param name="startRotation">The angle to start the rotation</param>
	/// <param name="endRotation">The angle to end the rotation</param>
	/// <param name="progression">Progression to transition rotation</param>
	/// <param name="pivotPosition">Position of pivot to rotate the
	/// element</param>
	public ActionRotate(float startRotation, float endRotation,
		IProgression progression, Vector2 pivotPosition)
	{
		totalMoveTime = 0;
		progressions = new List<IProgression>();
		progressions.Add(progression);
		transitionTimes = new List<float>();
		progression.GetProgressionTime();
		startRotations.Add(startRotation);
		startRotations.Add(endRotation);
		pivotPositions.Add(pivotPosition);
	}

	/// <summary>
	/// Adds action rotate with simple input
	/// </summary>
	/// <param name="startRotation">The angle to start the rotation</param>
	/// <param name="endRotation">The angle to end the rotation</param>
	/// <param name="progression">Progression to transition rotation</param>
	/// <returns>Same action with appended input action</returns>
	public ActionRotate AddAction(float startRotation, float endRotation,
		IProgression progression)
	{
		progressions.Add(progression);
		transitionTimes.Add(progression.GetProgressionTime());
		startRotations.Add(startRotation);
		endRotations.Add(endRotation);
		pivotPositions.Add(new Vector2(0, 0));
		totalMoveTime += transitionTimes[transitionTimes.Count - 1];
		return this;
	}

	/// <summary>
	/// Adds action rotate with complex input
	/// </summary>
	/// <param name="startRotation">The angle to start the rotation</param>
	/// <param name="endRotation">The angle to end the rotation</param>
	/// <param name="progression">Progression to transition rotation</param>
	/// <param name="pivotPosition">Position of pivot to rotate the
	/// element</param>
	/// <returns>Same action with appended input action</returns>
	public ActionRotate AddAction(float startRotation, float endRotation,
		IProgression progression, Vector2 pivotPosition)
	{
		progressions.Add(progression);
		transitionTimes.Add(progression.GetProgressionTime());
		startRotations.Add(startRotation);
		endRotations.Add(endRotation);
		pivotPositions.Add(pivotPosition);
		totalMoveTime += transitionTimes[transitionTimes.Count - 1];
		return this;
	}

	/// <summary>
	/// Whether the entire action has been completed at the given time
	/// </summary>
	/// <param name="time">Time to test if action is complete</param>
	/// <returns>Whether the entire action has been completed at the given
	/// time</returns>
	public bool ActionComplete(float time)
	{
		return totalMoveTime <= time;
	}

	/// <summary>
	/// Gets the total time to complete the action
	/// </summary>
	/// <returns>Total time for action to complete</returns>
	public float TotalActionTime()
	{
		return totalMoveTime;
	}

	/// <summary>
	/// Gets the position of the pivot given the specified time
	/// </summary>
	/// <param name="time">Time through action</param>
	/// <returns>The position of the pivot at the given time</returns>
	public Vector2 GetPivotPosition(float time)
	{

		// Keeps track of time through the individual actions
		float pivotTime = time;

		// Edge cases
		if (pivotTime <= 0)
		{
			return pivotPositions[0];
		}
		if (pivotTime >= totalMoveTime)
		{
			return pivotPositions[pivotPositions.Count - 1];
		}

		// Loops through each individual action
		for (int i = 0; i < transitionTimes.Count; i++)
		{

			// If time is within individual action, then get the pivot position
			// for the individual action
			if (transitionTimes[i] > pivotTime)
			{
				return pivotPositions[i];
			}
			pivotTime -= transitionTimes[i];
		}

		// Shouldn't be reached, default value for error case
		return new Vector2(0, 0);
	}

	/// <summary>
	/// Gets the rotation of the element at the spcified time
	/// </summary>
	/// <param name="time">Time through action</param>
	/// <returns>Rotation of the element at the given time</returns>
	public float GetElementRotation(float time)
	{

		// Keeps track of time through the individual actions
		float pivotTime = time;

		// Edge cases
		if (pivotTime <= 0)
		{
			return startRotations[0];
		}
		if (pivotTime >= totalMoveTime)
		{
			return endRotations[endRotations.Count - 1];
		}

		// Loops through each individual action
		for (int i = 0; i < transitionTimes.Count; i++)
		{

			// If time is within individual action, then get rotation from time
			// through the individual action
			if (transitionTimes[i] > pivotTime)
			{
				float rotationPercent =
					progressions[i].GetProgressionPercent(pivotTime);
				return startRotations[i] * (1 - rotationPercent) +
					endRotations[i] * rotationPercent;
			}
			pivotTime -= transitionTimes[i];
		}

		// Shouldn't be reached, default value for error case
		return 0;
	}

}
