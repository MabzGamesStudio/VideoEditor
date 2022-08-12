using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditElementValueScript : MonoBehaviour
{
	public float timeChange;
}


[CustomEditor(typeof(EditElementValueScript))]
public class EditElementValues : Editor
{
	private RectangleLineStill[] scripts;

	private GameObject parentGameObject;

	private EditElementValueScript parentScript;

	void OnEnable()
	{
		parentScript = (EditElementValueScript)target;
		parentGameObject = parentScript.gameObject;
		scripts = parentGameObject.GetComponentsInChildren<RectangleLineStill>();
	}

	private void EditAction()
	{
		float incrementTimes = parentScript.timeChange;
		for (int i = 0; i < scripts.Length; i++)
		{
			scripts[i].times += new Vector2(incrementTimes, incrementTimes);
		}
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Execute Edit Action"))
		{
			EditAction();
		}
	}
}
