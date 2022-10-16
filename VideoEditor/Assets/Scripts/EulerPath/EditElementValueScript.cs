using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Script to edit multiple element values at once based on a custom algorithm
/// </summary>
public class EditElementValueScript : MonoBehaviour
{

	/// <summary>
	/// Float to be used in the editor involved in the edit actions
	/// </summary>
	public float editFloat;

}

/// <summary>
/// Editor for the EditElementValueScript
/// </summary>
[CustomEditor(typeof(EditElementValueScript))]
public class EditElementValues : Editor
{

	/// <summary>
	/// All of the child circle node elements
	/// </summary>
	private CircleNodeElement[] nodeScripts;

	/// <summary>
	/// All of the child basic line elements
	/// </summary>
	private BasicLine[] lineScripts;

	/// <summary>
	/// Parent Game object whose children's element scripts will be edited
	/// </summary>
	private GameObject parentGameObject;

	/// <summary>
	/// Parent Game object script whose children's element scripts
	/// will be edited
	/// </summary>
	private EditElementValueScript parentScript;

	/// <summary>
	/// Initialize the gameobjects and scripts for editing
	/// </summary>
	void OnEnable()
	{
		parentScript = (EditElementValueScript)target;
		parentGameObject = parentScript.gameObject;
		nodeScripts = parentGameObject
			.GetComponentsInChildren<CircleNodeElement>();
		lineScripts = parentGameObject
			.GetComponentsInChildren<BasicLine>();
	}

	/// <summary>
	/// Executes the edit action 1
	/// </summary>
	private void EditAction1()
	{

		// Increments all node times by the editFloat value
		float incrementTimes = parentScript.editFloat;
		for (int i = 0; i < nodeScripts.Length; i++)
		{
			nodeScripts[i].waitTime += incrementTimes;
		}
	}

	/// <summary>
	/// Executes the edit action 1
	/// </summary>
	private void EditAction2()
	{

		// Increments all basic line times by the editFloat value
		float incrementTimes = parentScript.editFloat;
		for (int i = 0; i < lineScripts.Length; i++)
		{
			lineScripts[i].times += new Vector2(incrementTimes, incrementTimes);
		}
	}

	/// <summary>
	/// Executes the edit action 1
	/// </summary>
	private void EditAction3()
	{

	}

	/// <summary>
	/// Buttons to execute any of the Edit Actions
	/// </summary>
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Execute Edit Action 1"))
		{
			EditAction1();
		}
		if (GUILayout.Button("Execute Edit Action 2"))
		{
			EditAction2();
		}
		if (GUILayout.Button("Execute Edit Action 3"))
		{
			EditAction3();
		}
	}

}