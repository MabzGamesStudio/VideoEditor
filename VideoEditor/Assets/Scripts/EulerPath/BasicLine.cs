using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

/// <summary>
/// A line that extends from one point to another
/// </summary>
public class BasicLine : Element
{

	/// <summary>
	/// Start position of the line
	/// </summary>
	public Vector2 start;

	/// <summary>
	/// End position of the line
	/// </summary>
	public Vector2 end;

	/// <summary>
	/// With of the line
	/// </summary>
	public float width;

	/// <summary>
	/// Whether to use ease when extending from start to end
	/// </summary>
	public bool useEase;

	/// <summary>
	/// The time to wait and the time to extend from start to end
	/// (wait, entend)
	/// </summary>
	public Vector2 times;

	/// <summary>
	/// Amount of ease to use from 0 to 1 (no ease to full ease)
	/// </summary>
	public float easePercent;

	/// <summary>
	/// Progression type for the line
	/// </summary>
	private IProgression lineProgression;

	/// <summary>
	/// The line initializes transfrom info and line progression
	/// </summary>
	public override void InitialElement()
	{

		// Moves the line to the right spot, size, and angle
		transform.localPosition = GetPosition(start, end);
		transform.localScale = new Vector3(GetLength(start, end), width);
		transform.localEulerAngles = new Vector3(0, 0, GetAngle(start, end));

		// The time the line spends extending
		float totalTime = times.y - times.x;

		// Uses ease percent with ease progression if using ease
		if (useEase)
		{
			lineProgression = new EaseInOut(
				totalTime * easePercent / 2, totalTime);
		}
		else
		{
			lineProgression = new ConstantProgression(totalTime);
		}
	}

	/// <summary>
	/// Before the element update the scale and position are changed to reflect
	/// the progress of the line from start to finish
	/// </summary>
	/// <param name="time"></param>
	protected override void PreElementUpdate(float time)
	{

		// Keeps progression time between wait time and finish time
		float progressionTime = Mathf.Max(0, time - times.x);
		progressionTime = Mathf.Min(times.y - times.x, progressionTime);

		// Percent of line progression from 0 to 1
		float progressionStartPercent = lineProgression
			.GetProgressionPercent(progressionTime);

		// The position of the end of the line
		Vector2 updateEnd =
			start * (1 - progressionStartPercent) +
			end * progressionStartPercent;

		// Moves the rectangle into the updated line position
		transform.localPosition = GetPosition(start, updateEnd);
		transform.localScale = new Vector3(
			GetLength(start, updateEnd), width);
		transform.localEulerAngles = new Vector3(
			0, 0, GetAngle(start, updateEnd));
	}

	/// <summary>
	/// Gets the angle of the line direction in degrees
	/// </summary>
	/// <param name="startPoint">Start position of the line</param>
	/// <param name="endPoint">End position of the line</param>
	/// <returns>Angle of the line in degrees</returns>
	public float GetAngle(Vector2 startPoint, Vector2 endPoint)
	{

		// If the angle is straight up/down, that angle is returned directly
		if (startPoint.x == endPoint.x)
		{
			return 90 * (startPoint.y > endPoint.y ? -1 : 1);
		}

		// Arctangent of the angle is calculated from the start and end positions
		float tanAngle =
			Mathf.Atan((endPoint.y - startPoint.y)
			/ (endPoint.x - startPoint.x));

		// If the angle is out of the range [-pi/2, pi/2], then the angle is
		// increased by pi to account for the other half arctan doesn't output
		if (startPoint.x > endPoint.x)
		{
			tanAngle += Mathf.PI;
		}

		// The angle is converted from radians to degress and returned
		return tanAngle * (180f / Mathf.PI);
	}

	/// <summary>
	/// Gets the length of the line from the start and end points
	/// </summary>
	/// <param name="startPoint">Start position of the line</param>
	/// <param name="endPoint">End position of the line</param>
	/// <returns>The length of the given line</returns>
	public float GetLength(Vector2 startPoint, Vector2 endPoint)
	{
		return Mathf.Sqrt(Mathf.Pow(startPoint.x - endPoint.x, 2) + Mathf.Pow(startPoint.y - endPoint.y, 2));
	}

	/// <summary>
	/// Gets the position of the center of the line
	/// </summary>
	/// <param name="startPoint">Start position of the line</param>
	/// <param name="endPoint">End position of the line</param>
	/// <returns>The center position of the line</returns>
	public Vector2 GetPosition(Vector2 startPoint, Vector2 endPoint)
	{
		return new Vector2((startPoint.x + endPoint.x) / 2, (startPoint.y + endPoint.y) / 2);
	}

	/// <summary>
	/// Sets the line length to full lenth
	/// </summary>
	public void SetFullLine()
	{
		transform.localPosition = GetPosition(start, end);
		transform.localScale = new Vector3(GetLength(start, end), width);
		transform.localEulerAngles = new Vector3(0, 0, GetAngle(start, end));
	}

	/// <summary>
	/// Switches the positions of the start and end points
	/// </summary>
	public void SwapStartEndPoints()
	{
		Vector2 temp = new Vector2(start.x, start.y);
		start = end;
		end = temp;
		transform.localEulerAngles = new Vector3(0, 0, GetAngle(start, end));
	}

	/// <summary>
	/// Gets the total time it takes for the line to fully extend
	/// </summary>
	/// <returns></returns>
	public override float GetTotalActionTime()
	{
		return times.y;
	}

}

/// <summary>
/// Editor for the basic line script
/// </summary>
[CustomEditor(typeof(BasicLine))]
[CanEditMultipleObjects]
public class BasicLineEditor : Editor
{

	/// <summary>
	/// Whether to show the speed options foldout
	/// </summary>
	private bool showSpeedOptions;

	/// <summary>
	/// Speed for the line to extend
	/// </summary>
	private float lineSpeed;

	/// <summary>
	/// Wait time before the line extends
	/// </summary>
	private float startTime;

	/// <summary>
	/// Script of the basic line
	/// </summary>
	private BasicLine thisScript;

	/// <summary>
	/// Initialize the line script
	/// </summary>
	void OnEnable()
	{
		thisScript = (BasicLine)target;
	}

	/// <summary>
	/// Has actions for editing lines in custom ways
	/// </summary>
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		// Button visually shows the line position, scale, and
		// angle updated in the editor
		if (GUILayout.Button("Update Line"))
		{
			thisScript.SetFullLine();
		}

		// Button sets the inspector information to the current angle and position
		if (GUILayout.Button("Update Info"))
		{

			// Gets the position, angle, and length of the line
			Vector2 position = new Vector2(
				thisScript.transform.localPosition.x,
				thisScript.transform.localPosition.y);
			float length = thisScript.transform.localScale.x;
			float angle = thisScript.transform.localEulerAngles.z * Mathf.PI / 180f;

			// Trig to calculate the line start and end points
			thisScript.start = new Vector2(
				Mathf.Round(1000 * (position.x + length / 2f * Mathf.Cos(angle))) / 1000f,
				Mathf.Round(1000 * (position.y + length / 2f * Mathf.Sin(angle))) / 1000f);
			thisScript.end = new Vector2(
				Mathf.Round(1000 * (position.x - length / 2f * Mathf.Cos(angle))) / 1000f,
				Mathf.Round(1000 * (position.y - length / 2f * Mathf.Sin(angle))) / 1000f);

			// Sets the width value to the scale width
			thisScript.width = thisScript.transform.localScale.y;
		}

		// Button swaps the start and end points of the line
		if (GUILayout.Button("Swap Start And End Points"))
		{
			thisScript.SwapStartEndPoints();
		}

		// Foldout that shows the custom line speed options
		showSpeedOptions = EditorGUILayout
			.Foldout(showSpeedOptions, "Speed Options");
		if (showSpeedOptions)
		{

			// The start time and line speed float fields
			lineSpeed = EditorGUILayout.FloatField("Line Speed", lineSpeed);
			startTime = EditorGUILayout.FloatField("Start Time", startTime);

			// Button updates the line wait and progress times based on
			// constant speed with no ease
			if (GUILayout.Button("Update time from speed"))
			{
				thisScript.useEase = false;
				float distance = thisScript.transform.localScale.x;
				thisScript.times = new Vector2(
					startTime, startTime + distance / lineSpeed);
			}
		}
	}

}