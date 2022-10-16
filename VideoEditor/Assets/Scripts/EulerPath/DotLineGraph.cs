using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Information for a dot and line graph animation
/// </summary>
public class DotLineGraph : MonoBehaviour
{

	/// <summary>
	/// The positions of the nodes on the graph
	/// </summary>
	public Vector2[] nodePositions;

	/// <summary>
	/// Edge connections based on the node indexes
	/// ie: (0, 3) connects from node 0 to node 3
	/// </summary>
	public Vector2Int[] edgeConnections;

	/// <summary>
	/// Color of the nodes
	/// </summary>
	public Color nodeColor;

	/// <summary>
	/// Color of the edges
	/// </summary>
	public Color edgeColor;

	/// <summary>
	/// Time before the start of the graph animation
	/// </summary>
	public float waitTime;

	/// <summary>
	/// Time for node to pop into existence
	/// </summary>
	public float nodePopTime;

	/// <summary>
	/// Time for edge to extend from one node to the other
	/// </summary>
	public float edgeSpreadTime;

	/// <summary>
	/// Width of the edges
	/// </summary>
	public float edgeWidth;

	/// <summary>
	/// Diameter of the node circle
	/// </summary>
	public float nodeSize;

	/// <summary>
	/// Comma separated list of nodes to expand together in stages
	/// where each stage is denoted by the string list element
	/// ie: ["0,1", "2,3"], expand nodes 1 and 2 at the same
	/// time then expand nodes 2 and 3 after they have completed
	/// </summary>
	public List<string> nodeStages;

	/// <summary>
	/// Comma separated list of edges to expand together in stages
	/// where each stage is denoted by the string list element
	/// ie: ["0,1", "2,3"], expand edges 1 and 2 at the same
	/// time then expand edges 2 and 3 after they have completed
	/// </summary>
	public List<string> edgeStages;

	/// <summary>
	/// Basic line prefab
	/// </summary>
	public GameObject edgeElementPrefab;

	/// <summary>
	/// Node prefab
	/// </summary>
	public GameObject nodeElementPrefab;

}

/// <summary>
/// Custom editor for the dot line graph animation creator
/// </summary>
[CustomEditor(typeof(DotLineGraph))]
public class DotLineGraphEditor : Editor
{

	/// <summary>
	/// The positions of the nodes on the graph
	/// </summary>
	private Vector2[] nodePositions;

	/// <summary>
	/// Edge connections based on the node indexes
	/// ie: (0, 3) connects from node 0 to node 3
	/// </summary>
	private Vector2Int[] edgeConnections;

	/// <summary>
	/// Color of the nodes
	/// </summary>
	private Color nodeColor;

	/// <summary>
	/// Color of the edges
	/// </summary>
	private Color edgeColor;

	/// <summary>
	/// Time before the start of the graph animation
	/// </summary>
	private float waitTime;

	/// <summary>
	/// Time for node to pop into existence
	/// </summary>
	private float nodePopTime;

	/// <summary>
	/// Time for edge to extend from one node to the other
	/// </summary>
	private float edgeSpreadTime;

	/// <summary>
	/// Width of the edges
	/// </summary>
	private float edgeWidth;

	/// <summary>
	/// Diameter of the node circle
	/// </summary>
	private float nodeSize;

	/// <summary>
	/// Comma separated list of nodes to expand together in stages
	/// where each stage is denoted by the string list element
	/// ie: ["0,1", "2,3"], expand nodes 1 and 2 at the same
	/// time then expand nodes 2 and 3 after they have completed
	/// </summary>
	private List<string> nodeStages;

	/// <summary>
	/// Comma separated list of edges to expand together in stages
	/// where each stage is denoted by the string list element
	/// ie: ["0,1", "2,3"], expand edges 1 and 2 at the same
	/// time then expand edges 2 and 3 after they have completed
	/// </summary>
	private List<string> edgeStages;

	/// <summary>
	/// Basic line prefab
	/// </summary>
	private GameObject edgeElementPrefab;

	/// <summary>
	/// Node prefab
	/// </summary>
	private GameObject nodeElementPrefab;

	/// <summary>
	/// Dot line graph game object that this editor affects
	/// </summary>
	private DotLineGraph alias;

	/// <summary>
	/// Updates all of the private variables to the game object data
	/// </summary>
	private void UpdateVariables()
	{
		nodePositions = alias.nodePositions;
		edgeConnections = alias.edgeConnections;
		nodeColor = alias.nodeColor;
		edgeColor = alias.edgeColor;
		waitTime = alias.waitTime;
		nodePopTime = alias.nodePopTime;
		edgeSpreadTime = alias.edgeSpreadTime;
		edgeWidth = alias.edgeWidth;
		nodeSize = alias.nodeSize;
		nodeStages = alias.nodeStages;
		edgeStages = alias.edgeStages;
		edgeElementPrefab = alias.edgeElementPrefab;
		nodeElementPrefab = alias.nodeElementPrefab;
	}

	/// <summary>
	/// When the editor is enabled, init the dot line graph game object
	/// </summary>
	private void OnEnable()
	{
		alias = (DotLineGraph)target;
	}

	/// <summary>
	/// When the Update Element button is pressed, update the children elements
	/// of the dot line graph game object
	/// </summary>
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();


		// When the button is pressed update the children elements
		if (GUILayout.Button("Update Element"))
		{

			// Update private variables to data in game object
			UpdateVariables();

			// Deletes all child game objects
			Transform[] children = alias.GetComponentsInChildren<Transform>();
			for (int i = 1; i < children.Length; i++)
			{
				if (children[i] != null)
				{
					DestroyImmediate(children[i].gameObject);
				}
			}

			// Create all node elements
			CircleNodeElement[] nodeScripts =
				new CircleNodeElement[nodePositions.Length];

			for (int i = 0; i < nodePositions.Length; i++)
			{

				// Create the node and set its name, parent, position, and size
				GameObject addNode = Instantiate(nodeElementPrefab);
				addNode.transform.parent = alias.transform;
				addNode.transform.localPosition =
					new Vector3(nodePositions[i].x, nodePositions[i].y, 0);
				addNode.transform.localScale =
					new Vector3(nodeSize, nodeSize, 0);
				addNode.name = "Node (" + i + ")";

				// Saves node script variable and node pop time
				nodeScripts[i] = addNode.GetComponent<CircleNodeElement>();
				nodeScripts[i].popTime = nodePopTime;

				// Sets the color of the node
				addNode.GetComponent<SpriteRenderer>().color = nodeColor;
			}

			// Create all edge elements
			BasicLine[] edgeScripts = new BasicLine[edgeConnections.Length];

			for (int i = 0; i < edgeConnections.Length; i++)
			{

				// Create the edge and set its name and parent
				GameObject addEdge = Instantiate(edgeElementPrefab);
				addEdge.transform.parent = alias.transform;
				addEdge.name = "Edge (" + i + ")";

				// Sets the size, position, and rotation of the edge
				edgeScripts[i] = addEdge.GetComponent<BasicLine>();
				edgeScripts[i].start = nodePositions[edgeConnections[i][0]];
				edgeScripts[i].end = nodePositions[edgeConnections[i][1]];
				edgeScripts[i].width = edgeWidth;
				edgeScripts[i].useEase = true;
				edgeScripts[i].SetFullLine();

				// Sets the color of the edge
				addEdge.GetComponent<SpriteRenderer>().color = edgeColor;
			}

			// The parsed stages of the nodes and edges
			List<List<int>> nodeStageParse = new List<List<int>>();
			List<List<int>> edgeStageParse = new List<List<int>>();

			// Parses the list of strings for the node stages
			for (int i = 0; i < nodeStages.Count; i++)
			{

				// The parsed list of ints in the current stage
				List<int> stageNums = new List<int>();

				// Line is copied from the stages string list
				string line = nodeStages[i];

				// Saves the length of the line string
				int lineLength = line.Length;

				// Parses the string of its ints
				for (int j = 0; j < lineLength; j++)
				{

					// Gets the next int in the comma separated list
					if (line.IndexOf(",") == -1)
					{
						try
						{
							stageNums.Add(int.Parse(line.Substring(0)));
						}
						catch { }
						break;
					}
					stageNums.Add(
						int.Parse(line.Substring(0, line.IndexOf(","))));
					line = line.Substring(line.IndexOf(",") + 1);
				}
				nodeStageParse.Add(stageNums);
			}

			// Parses the list of strings for the edge stages
			for (int i = 0; i < edgeStages.Count; i++)
			{

				// The parsed list of ints in the current stage
				List<int> stageNums = new List<int>();

				// Line is copied from the stages string list
				string line = edgeStages[i];

				// Saves the length of the line string
				int lineLength = line.Length;

				// Parses the string of its ints
				for (int j = 0; j < lineLength; j++)
				{

					// Gets the next int in the comma separated list
					if (line.IndexOf(",") == -1)
					{
						try
						{
							stageNums.Add(int.Parse(line.Substring(0)));
						}
						catch { }
						break;
					}
					stageNums.Add(
						int.Parse(line.Substring(0, line.IndexOf(","))));
					line = line.Substring(line.IndexOf(",") + 1);
				}
				edgeStageParse.Add(stageNums);
			}

			// Sets the time of each node based on the node stage
			for (int i = 0; i < nodeStageParse.Count; i++)
			{
				for (int j = 0; j < nodeStageParse[i].Count; j++)
				{

					// The first node pops up after wait time
					if (i == 0)
					{
						nodeScripts[nodeStageParse[i][j]].waitTime = waitTime;
					}

					// Node pops up after a node pop +
					// each edge spread time per stage
					else
					{
						nodeScripts[nodeStageParse[i][j]].waitTime =
							waitTime + nodePopTime + edgeSpreadTime * (i - 1);
					}
				}
			}

			// Sets the time of each edge based on the edge stage
			for (int i = 0; i < edgeStageParse.Count; i++)
			{
				for (int j = 0; j < edgeStageParse[i].Count; j++)
				{

					// Edge starts spread based on stage iteration and ends spread
					// after the edge spread time after that
					edgeScripts[edgeStageParse[i][j]].times.x =
						waitTime + nodePopTime + edgeSpreadTime * i;
					edgeScripts[edgeStageParse[i][j]].times.y =
						edgeScripts[edgeStageParse[i][j]].times.x + edgeSpreadTime;
				}
			}

			// Update the scene manager with the new updated element scripts
			GameObject.Find("SceneManager").GetComponent<SceneManager>().SetAllElements();
		}
	}

}