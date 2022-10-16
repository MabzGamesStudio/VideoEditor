using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Mark spins and fades into existence
/// </summary>
public class SpinCheckElement : Element
{

	/// <summary>
	/// Amount of degrees to turn while spinning
	/// </summary>
	public float spinAmount;

	/// <summary>
	/// Time it takes to complete the spin
	/// </summary>
	public float spinTime;

	/// <summary>
	/// Time before the element begins fading/spinning
	/// </summary>
	public float waitTime;

	/// <summary>
	/// Transparent color as starting color
	/// </summary>
	private Color transparent = new Color(0, 0, 0, 0);

	/// <summary>
	/// Mark has a spin action and color action to fade in
	/// </summary>
	public override void InitialElement()
	{

		// Ease and constant progressions
		IProgression easeProgression = new EaseInOut(spinTime / 2, spinTime);
		IProgression waitProgression = new ConstantProgression(waitTime);

		// Mark waits, then fades from transparent to solid color
		colorTransition = new ActionColor()
			.AddAction(transparent, transparent, waitProgression)
			.AddAction(transparent, Color.white, easeProgression);

		// Mark waits, then spins
		rotateTransition = new ActionRotate()
			.AddAction(0, 0, waitProgression)
			.AddAction(-spinAmount, 0, easeProgression);
	}

}