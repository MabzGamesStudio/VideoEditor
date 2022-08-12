using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Highest overall manager of the control of the scene
/// </summary>
public class SceneManager : MonoBehaviour
{

	/// <summary>
	/// Test circle 1
	/// </summary>
	public Element[] animationElements;

	/// <summary>
	/// The action manager script controls circle movements
	/// </summary>
	private ActionManager actionManager;

	/// <summary>
	/// The parent that contains all elements to be animated
	/// </summary>
	public GameObject elementsParent;

	/// <summary>
	/// On start initialize the action manager with the two circles actions
	/// </summary>
	private void Start()
	{

		// Initialize action manager script
		actionManager = GetComponent<ActionManager>();

		// Set the action elements to the two circles
		List<Element> elements = new List<Element>();
		for (int i = 0; i < animationElements.Length; i++)
		{
			elements.Add(animationElements[i]);
		}
		actionManager.SetElements(elements);
	}

	/// <summary>
	/// Sets all of the animation elements to all components in the children of
	/// the elementsParent game object
	/// </summary>
	public void SetAllElements()
	{
		animationElements = elementsParent.GetComponentsInChildren<Element>();
	}

}


[CustomEditor(typeof(SceneManager))]
public class SceneManagerEditor : Editor
{

	private SceneManager thisScript;

	void OnEnable()
	{
		thisScript = (SceneManager)target;
	}

	public override void OnInspectorGUI()
	{


		base.OnInspectorGUI();
		if (GUILayout.Button("Update Elements"))
		{
			thisScript.SetAllElements();
		}
	}
}
