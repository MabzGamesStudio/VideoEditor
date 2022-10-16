using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

/// <summary>
/// Chanages the text
/// </summary>
public class TextChangeElement : Element
{

	/// <summary>
	/// The texts to be displayed in order
	/// </summary>
	public string[] texts;

	/// <summary>
	/// The times for each of the respective texts to show
	/// </summary>
	public float[] textTimes;


	/// <summary>
	/// The text mesh component of the element
	/// </summary>
	private TextMeshProUGUI textMesh;

	/// <summary>
	/// Initialize the text mesh component and its text value
	/// </summary>
	public override void InitialElement()
	{
		textMesh = GetComponentInChildren<TextMeshProUGUI>();
		textMesh.text = texts[0];
	}

	/// <summary>
	/// Set the text value before updating the element
	/// </summary>
	/// <param name="time">Time through the animation</param>
	protected override void PreElementUpdate(float time)
	{

		// Find the index of the animation time in the text array
		int i;
		float timeSum = 0f;
		for (i = 0; i < textTimes.Length; i++)
		{
			timeSum += textTimes[i];
			if (timeSum >= time)
			{
				break;
			}
		}

		// Set the text to the proper value based on the animation time
		textMesh.text = texts[Mathf.Min(i, texts.Length - 1)];
	}

}

/// <summary>
/// Custom editor for the time change element
/// </summary>
[CustomEditor(typeof(TextChangeElement))]
[CanEditMultipleObjects]
public class TextChangeElementEditor : Editor
{

	/// <summary>
	/// Whether to use a constant time step in the text change
	/// </summary>
	private bool constantTimeStep = false;

	/// <summary>
	/// Time step to use for each text value
	/// </summary>
	private float timeStep = 1;

	/// <summary>
	/// Script of the text change element
	/// </summary>
	private TextChangeElement thisScript;

	/// <summary>
	/// Initializes the text change element script
	/// </summary>
	void OnEnable()
	{
		thisScript = (TextChangeElement)target;
	}

	/// <summary>
	/// Makes script changes based on changes in the editor
	/// </summary>
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		// Toggle for the use constant time step
		constantTimeStep = EditorGUILayout.Toggle(
			"Constant Time Step",
			constantTimeStep);

		// Show time step variable if using constant time step
		if (constantTimeStep)
		{

			// If the time step variable changes from last editor update
			float lastTimeStep = timeStep;
			timeStep = EditorGUILayout.FloatField("Time Step", timeStep);
			if (lastTimeStep != timeStep)
			{

				// Update all times in the time change element script
				// to the value of the time step
				float[] newTimes = new float[thisScript.texts.Length];
				for (int i = 0; i < newTimes.Length; i++)
				{
					newTimes[i] = timeStep;
				}
				thisScript.textTimes = newTimes;
			}
		}
	}

}