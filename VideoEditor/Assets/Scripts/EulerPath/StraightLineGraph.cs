using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Information for a stright line graph animation
/// </summary>
public class StraightLineGraph : MonoBehaviour
{

	/// <summary>
	/// The ordered list of pionts the graph goes to
	/// (Points must be orthogonal)
	/// </summary>
	public Vector2[] endpointPositions;

	/// <summary>
	/// Color of the line
	/// </summary>
	public Color color;

	/// <summary>
	/// Width of the line
	/// </summary>
	public float edgeWidth;

	/// <summary>
	/// Time before the animation starts
	/// </summary>
	public float waitTime;

	/// <summary>
	/// Speed of the line progress in world units per second
	/// </summary>
	[HideInInspector]
	public float lineSpeed;

	/// <summary>
	/// Total time for the line to go from start endpoint to finish endpoint
	/// </summary>
	[HideInInspector]
	public float totalTime;

	/// <summary>
	/// Prefab of the edge element
	/// </summary>
	public GameObject edgeElementPrefab;

}

/// <summary>
/// Custom editor for the straight line graph animation
/// </summary>
[CustomEditor(typeof(StraightLineGraph))]
public class StraightLineGraphEditor : Editor
{

	/// <summary>
	/// The ordered list of pionts the graph goes to
	/// (Points must be orthogonal)
	/// </summary>
	private Vector2[] endpointPositions;

	/// <summary>
	/// Color of the line
	/// </summary>
	private Color color;

	/// <summary>
	/// Width of the line
	/// </summary>
	private float edgeWidth;

	/// <summary>
	/// Time before the animation starts
	/// </summary>
	private float waitTime;

	/// <summary>
	/// Speed of the line progress in world units per second
	/// </summary>
	private float lineSpeed;

	/// <summary>
	/// Total time for the line to go from start endpoint to finish endpoint
	/// </summary>
	private float totalTime;

	/// <summary>
	/// Prefab of the edge element
	/// </summary>
	private GameObject edgeElementPrefab;

	/// <summary>
	/// Stright line graph game object that this editor affects
	/// </summary>
	private StraightLineGraph alias;

	/// <summary>
	/// Whether to show line speed or total time variable
	/// </summary>
	private bool useSpeed = true;

	/// <summary>
	/// Distances from start to each endpoint
	/// </summary>
	private float[] lineDistances;

	/// <summary>
	/// Updates the variables to the values from the game object
	/// </summary>
	private void UpdateVariables()
	{
		endpointPositions = alias.endpointPositions;
		lineDistances = new float[endpointPositions.Length];
		color = alias.color;
		edgeWidth = alias.edgeWidth;
		waitTime = alias.waitTime;
		lineSpeed = alias.lineSpeed;
		totalTime = alias.totalTime;
		edgeElementPrefab = alias.edgeElementPrefab;
	}

	/// <summary>
	/// Sets the target to the stright line element
	/// </summary>
	public void OnEnable()
	{
		alias = (StraightLineGraph)target;
	}

	/// <summary>
	/// When the Update Element button is pressed, update the children elements
	/// of the stright line graph game object
	/// </summary>
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		// Update  variables
		UpdateVariables();

		// Toggle between showing line speed and total time
		useSpeed = EditorGUILayout.Toggle("Speed/Time", useSpeed);

		// If showing the line speed
		if (useSpeed)
		{

			// Set to see any change in the line speed variable
			float lastSpeed = lineSpeed;

			// Updates line speed float
			lineSpeed = EditorGUILayout.FloatField("Line Speed", lineSpeed);

			// If the line speed is changed, update total time dependent variable,
			// and update the game object with the new variables
			if (lastSpeed != lineSpeed)
			{
				if (lineSpeed == 0)
				{
					totalTime = 0;
				}
				else
				{
					totalTime = GetDistance() / lineSpeed;
				}
				alias.lineSpeed = lineSpeed;
				alias.totalTime = totalTime;
			}
		}

		// If showing the total time
		else
		{

			// Set to see any change in the total time variable
			float lastTotal = totalTime;

			// Updates total time float
			totalTime = EditorGUILayout.FloatField("Total Time", totalTime);

			// If the total time is changed, update line speed dependent variable,
			// and update the game object with the new variables
			if (lastTotal != totalTime)
			{
				if (totalTime == 0)
				{
					lineSpeed = 0;
				}
				else
				{
					lineSpeed = GetDistance() / totalTime;
				}
				alias.lineSpeed = lineSpeed;
				alias.totalTime = totalTime;
			}
		}

		// When the element update button is pressed, create the new line graph
		if (GUILayout.Button("Update Element"))
		{

			// Deletes all child game objects
			Transform[] children = alias.GetComponentsInChildren<Transform>();
			for (int i = 1; i < children.Length; i++)
			{
				if (children[i] != null)
				{
					DestroyImmediate(children[i].gameObject);
				}
			}

			// Create all edge elements
			BasicLine[] edgeScripts = new BasicLine[endpointPositions.Length];

			for (int i = 0; i < endpointPositions.Length - 1; i++)
			{

				// If the lines are not orthogonal, show an error and stop return
				if (endpointPositions[i].x != endpointPositions[i + 1].x &&
					endpointPositions[i].y != endpointPositions[i + 1].y)
				{
					Debug.LogError("Endpoints must be orthogonal up/down or left/right lines." +
						"Endpoints " + i + " and " + (i + 1) + " are diagonal.");
					return;
				}

				// Create the edge and set its name and parent
				GameObject addEdge = Instantiate(edgeElementPrefab);
				addEdge.transform.parent = alias.transform;
				addEdge.name = "Edge (" + i + ")";

				// Update the line width, and no ease for constant progress
				edgeScripts[i].width = edgeWidth;
				edgeScripts[i].useEase = false;

				// Sets the size, position, and rotation of the edge
				edgeScripts[i] = addEdge.GetComponent<BasicLine>();

				// direction of line: 0=UP, 1=RIGHT, 2=DOWN, 3=LEFT
				int lineType;

				// vertical line
				if (endpointPositions[i].x == endpointPositions[i + 1].x)
				{

					// line up
					if (endpointPositions[i].y < endpointPositions[i + 1].y)
					{
						lineType = 0;
					}

					// line down
					else
					{
						lineType = 2;
					}
				}

				// horizontal line
				else
				{

					// line right
					if (endpointPositions[i].x < endpointPositions[i + 1].x)
					{
						lineType = 1;
					}

					// line left
					else
					{
						lineType = 3;
					}
				}

				// Sets the line to start at the endpoint. The start line enpoints
				// extend by edgeWidth/2 on both sides to square off the corners
				edgeScripts[i].start = endpointPositions[i];

				// If the starting line, extend the start back
				if (i == 0)
				{
					switch (lineType)
					{
						case 0:
							edgeScripts[i].start += new Vector2(0, -edgeWidth / 2);
							break;
						case 1:
							edgeScripts[i].start += new Vector2(-edgeWidth / 2, 0);
							break;
						case 2:
							edgeScripts[i].start += new Vector2(0, edgeWidth / 2);
							break;
						case 3:
							edgeScripts[i].start += new Vector2(edgeWidth / 2, 0);
							break;
					}
				}

				// If !starting line, extend the start forward
				else
				{
					switch (lineType)
					{
						case 0:
							edgeScripts[i].start += new Vector2(0, edgeWidth / 2);
							break;
						case 1:
							edgeScripts[i].start += new Vector2(edgeWidth / 2, 0);
							break;
						case 2:
							edgeScripts[i].start += new Vector2(0, -edgeWidth / 2);
							break;
						case 3:
							edgeScripts[i].start += new Vector2(-edgeWidth / 2, 0);
							break;
					}
				}


				// The endpoints of the lines are extended
				edgeScripts[i].end = endpointPositions[i + 1];
				switch (lineType)
				{
					case 0:
						edgeScripts[i].end += new Vector2(0, edgeWidth / 2);
						break;
					case 1:
						edgeScripts[i].end += new Vector2(edgeWidth / 2, 0);
						break;
					case 2:
						edgeScripts[i].end += new Vector2(0, -edgeWidth / 2);
						break;
					case 3:
						edgeScripts[i].end += new Vector2(-edgeWidth / 2, 0);
						break;
				}

				// Update the visual line with the new endpoints
				edgeScripts[i].SetFullLine();

				// Sets the color of the edge
				addEdge.GetComponent<SpriteRenderer>().color = color;

				// Updates the lineDistances variable
				GetDistance();

				// Time up to this edge start
				float timeToLine = waitTime + lineDistances[i] / lineSpeed;

				// Length of this edge
				float edgeLength = Vector2.Distance(endpointPositions[i], endpointPositions[i + 1]);

				// Update the time based on line speed, edge length, and start time
				edgeScripts[i].times = new Vector2(timeToLine, timeToLine + edgeLength / lineSpeed);
			}

			// Update the scene manager with the new updated element scripts
			GameObject.Find("SceneManager").GetComponent<SceneManager>().SetAllElements();
		}
	}

	/// <summary>
	/// Gets the total distance of the entire graph line and updates
	/// the lineDistances array
	/// </summary>
	/// <returns>Total distance of the entire graph line</returns>
	private float GetDistance()
	{

		// Distance init to 0
		float dist = 0;

		// Loops through each line in the graph
		for (int i = 0; i < endpointPositions.Length - 1; i++)
		{

			// Total graph line distance up to this line
			lineDistances[i] = dist;

			// Total graph distance incremented by edge length
			dist += Vector2.Distance(endpointPositions[i], endpointPositions[i + 1]);
		}
		return dist;
	}

}