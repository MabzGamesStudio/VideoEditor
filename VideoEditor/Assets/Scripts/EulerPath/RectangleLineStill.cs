using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class RectangleLineStill : Element
{

	public Vector2 start;

	public Vector2 end;

	public float width;

	public bool useEase;

	public Vector2 times;

	public float easePercent;

	private IProgression lineProgression;


	public override void InitialElement()
	{
		transform.localPosition = GetPosition(start, end);
		transform.localScale = new Vector3(GetLength(start, end), width);
		transform.localEulerAngles = new Vector3(0, 0, GetAngle(start, end));



		float totalTime = times.y - times.x;
		if (useEase)
		{
			lineProgression = new EaseInOut(totalTime * easePercent / 2, totalTime);
		}
		else
		{
			lineProgression = new ConstantProgression(totalTime);
		}
	}

	protected override void PreElementUpdate(float time)
	{

		float progressionTime = Mathf.Max(0, time - times.x);
		progressionTime = Mathf.Min(times.y - times.x, progressionTime);

		float progressionStartPercent = lineProgression.GetProgressionPercent(progressionTime);

		Vector2 updateEnd = start * (1 - progressionStartPercent) + end * progressionStartPercent;


		transform.localPosition = GetPosition(start, updateEnd);
		transform.localScale = new Vector3(GetLength(start, updateEnd), width);
		transform.localEulerAngles = new Vector3(0, 0, GetAngle(start, updateEnd));
	}

	private float GetAngle(Vector2 startPoint, Vector2 endPoint)
	{
		if (startPoint.x == endPoint.x)
		{
			return 90 * (startPoint.y > endPoint.y ? -1 : 1);
		}

		float tanAngle = Mathf.Atan((endPoint.y - startPoint.y) / (endPoint.x - startPoint.x));

		if (startPoint.x > endPoint.x)
		{
			tanAngle += Mathf.PI;
		}

		return tanAngle * (180f / Mathf.PI);

	}

	private float GetLength(Vector2 startPoint, Vector2 endPoint)
	{
		return Mathf.Sqrt(Mathf.Pow(startPoint.x - endPoint.x, 2) + Mathf.Pow(startPoint.y - endPoint.y, 2));
	}

	private Vector2 GetPosition(Vector2 startPoint, Vector2 endPoint)
	{
		return new Vector2((startPoint.x + endPoint.x) / 2, (startPoint.y + endPoint.y) / 2);
	}

	public void SetFullLine()
	{
		transform.localPosition = GetPosition(start, end);
		transform.localScale = new Vector3(GetLength(start, end), width);
		transform.localEulerAngles = new Vector3(0, 0, GetAngle(start, end));
	}

	public override float GetTotalActionTime()
	{
		return times.y;
	}

}

[CustomEditor(typeof(RectangleLineStill))]
public class RectangleLineStillEditor : Editor
{

	private RectangleLineStill thisScript;

	void OnEnable()
	{
		thisScript = (RectangleLineStill)target;
	}

	public override void OnInspectorGUI()
	{


		base.OnInspectorGUI();
		if (GUILayout.Button("Update Line"))
		{
			thisScript.SetFullLine();
		}
	}
}