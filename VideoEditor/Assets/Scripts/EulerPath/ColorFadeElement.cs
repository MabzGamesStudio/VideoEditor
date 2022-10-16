using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fades the element between different colors
/// </summary>
public class ColorFadeElement : Element
{

	/// <summary>
	/// The times to fade from color to color
	/// </summary>
	public float[] timeSpans;

	/// <summary>
	/// The colors to fade between
	/// </summary>
	public Color[] colors;

	/// <summary>
	/// Initializes the color transitions
	/// </summary>
	public override void InitialElement()
	{

		// Initializes a default color action
		colorTransition = new ActionColor();

		// Adds the color fade action color by color from the list of colors
		for (int i = 0; i < Mathf.Min(timeSpans.Length, colors.Length - 1); i++)
		{
			colorTransition.AddAction(
				colors[i],
				colors[i + 1],
				new ConstantProgression(timeSpans[i]));
		}
	}

}