using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Highest overall manager of the control of the scene
/// </summary>
public class SceneManager : MonoBehaviour
{

	/// <summary>
	/// Test circle 1
	/// </summary>
	public Element circle1;

	/// <summary>
	/// Test circle 2
	/// </summary>
	public Element circle2;

	/// <summary>
	/// The action manager script controls circle movements
	/// </summary>
	private ActionManager actionManager;

	/// <summary>
	/// On start initialize the action manager with the two circles actions
	/// </summary>
	void Start()
	{

		// Initialize action manager script
		actionManager = GetComponent<ActionManager>();

		// Set the action elements to the two circles
		List<Element> elements = new List<Element>();
		elements.Add(circle1);
		elements.Add(circle2);
		actionManager.SetElements(elements);
	}

}
