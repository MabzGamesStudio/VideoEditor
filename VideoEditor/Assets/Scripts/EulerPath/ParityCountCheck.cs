using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Script to hold the information for the parity count mark
/// </summary>
public class ParityCountCheck : MonoBehaviour
{

	/// <summary>
	/// Time before the action begins
	/// </summary>
	public float waitTime;

	/// <summary>
	/// Number that it counts up to from 0
	/// </summary>
	public int numberCount;

	/// <summary>
	/// Time to count from 0 to numberCount
	/// </summary>
	public float countTime;

	/// <summary>
	/// Intermediate time after counting and before mark spin
	/// </summary>
	public float intermediateTime;

	/// <summary>
	/// Number of degrees the mark spins
	/// </summary>
	public float checkSpinAmount;

	/// <summary>
	/// Time for check to spin and fade in
	/// </summary>
	public float checkTime;

	/// <summary>
	/// Color background of even number
	/// </summary>
	public Color evenColor;

	/// <summary>
	/// Color background of odd number
	/// </summary>
	public Color oddColor;

	/// <summary>
	/// Sprite of the check mark
	/// </summary>
	public Sprite checkMark;

	/// <summary>
	/// Sprite of the x mark
	/// </summary>
	public Sprite xMark;

}

/// <summary>
/// Editor of the parity count check
/// </summary>
[CustomEditor(typeof(ParityCountCheck))]
[CanEditMultipleObjects]
public class ParityCountCheckEditor : Editor
{

	/// <summary>
	/// Time before the action begins
	/// </summary>
	private float waitTime;

	/// <summary>
	/// Number that it counts up to from 0
	/// </summary>
	private int numberCount;

	/// <summary>
	/// Time to count from 0 to numberCount
	/// </summary>
	private float countTime;

	/// <summary>
	/// Intermediate time after counting and before mark spin
	/// </summary>
	private float intermediateTime;

	/// <summary>
	/// Number of degrees the mark spins
	/// </summary>
	private float checkSpinAmount;

	/// <summary>
	/// Time for check to spin and fade in
	/// </summary>
	private float checkTime;

	/// <summary>
	/// Color background of even number
	/// </summary>
	private Color evenColor;

	/// <summary>
	/// Color background of odd number
	/// </summary>
	private Color oddColor;

	/// <summary>
	/// Script to edit game object
	/// </summary>
	private ParityCountCheck gameObjectAlias;

	/// <summary>
	/// Text change element of the parity check count
	/// </summary>
	private TextChangeElement textChange;

	/// <summary>
	/// Color fade element of the parity check count
	/// </summary>
	private ColorFadeElement colorFade;

	/// <summary>
	/// Spin check element of the parity check count
	/// </summary>
	private SpinCheckElement spinCheck;

	/// <summary>
	/// Sprite of the check mark
	/// </summary>
	private Sprite xMark;

	/// <summary>
	/// Sprite of the x mark
	/// </summary>
	private Sprite checkMark;

	/// <summary>
	/// Updates the private variables with the game object variables
	/// </summary>
	private void UpdateEditorVariables()
	{

		numberCount = gameObjectAlias.numberCount;
		countTime = gameObjectAlias.countTime;
		intermediateTime = gameObjectAlias.intermediateTime;
		checkSpinAmount = gameObjectAlias.checkSpinAmount;
		checkTime = gameObjectAlias.checkTime;
		waitTime = gameObjectAlias.waitTime;
		evenColor = gameObjectAlias.evenColor;
		oddColor = gameObjectAlias.oddColor;
		checkMark = gameObjectAlias.checkMark;
		xMark = gameObjectAlias.xMark;
	}

	/// <summary>
	/// Inits the parity check and its child element scripts
	/// </summary>
	private void OnEnable()
	{
		gameObjectAlias = (ParityCountCheck)target;
		textChange = gameObjectAlias.GetComponentInChildren<TextChangeElement>();
		colorFade = gameObjectAlias.GetComponentInChildren<ColorFadeElement>();
		spinCheck = gameObjectAlias.GetComponentInChildren<SpinCheckElement>();
	}

	/// <summary>
	/// On update of the Editor GUI
	/// </summary>
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		// When the update element button is pressed
		if (GUILayout.Button("Update Element"))
		{

			// Update the private variables to the game object variables
			UpdateEditorVariables();

			// Array from 0 to numberCount
			string[] countArray = new string[numberCount + 1];

			// Time spans for count numbers
			float[] countTimes = new float[numberCount + 1];

			// Number background colors to use
			Color[] flashColors = new Color[2 * (numberCount + 1)];

			// Number background color transition time spans
			float[] colorTimeSpans = new float[2 * (numberCount + 1) - 1];

			// Initialize beginning element time spans with wait time
			colorTimeSpans[0] = waitTime + countTime / (numberCount + 1);
			countTimes[0] = waitTime + countTime / (numberCount + 1);

			// Loops through all the counting number arrays
			for (int i = 0; i <= numberCount; i++)
			{

				// Sets the count text to the numbers 0 to numberCount
				countArray[i] = i.ToString();

				// Sets the count time span to the average number time over the countTime
				if (i != 0)
				{
					countTimes[i] = countTime / (numberCount + 1);
				}

				// Two colors are created per number to prevent fading from color to color
				// and instead has instant color transitions
				flashColors[2 * i] = (i % 2 == 0) ? evenColor : oddColor;
				flashColors[2 * i + 1] = (i % 2 == 0) ? evenColor : oddColor;

				// Sets the color time spans to immediately change color then wait
				if (i != numberCount)
				{
					colorTimeSpans[2 * i + 1] = 0f;
					colorTimeSpans[2 * i + 2] = countTime / (numberCount + 1);
				}


			}

			// Updates the text change text and time spans
			textChange.texts = countArray;
			textChange.textTimes = countTimes;

			// Updates the colors and their time spans
			colorFade.colors = flashColors;
			colorFade.timeSpans = colorTimeSpans;

			// Updates the mark variables
			spinCheck.spinAmount = checkSpinAmount;
			spinCheck.spinTime = checkTime;
			spinCheck.waitTime = waitTime + countTime + intermediateTime;
			spinCheck.gameObject.GetComponentInChildren<SpriteRenderer>().sprite
				= (numberCount % 2 == 0) ? checkMark : xMark;
		}
	}

}
