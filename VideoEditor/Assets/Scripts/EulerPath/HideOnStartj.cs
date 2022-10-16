using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the game object to inactive on play
/// </summary>
public class HideOnStartj : MonoBehaviour
{

	/// <summary>
	/// Sets the game object to inactive on play
	/// </summary>
	void Start()
	{
		gameObject.SetActive(false);
	}

}
