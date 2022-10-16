using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pans and/ or zooms the element and all children elements
/// </summary>
public class PanElement : Element
{

	/// <summary>
	/// Array of positions to pan to
	/// </summary>
	public Vector2[] positions;

	/// <summary>
	/// Array of sizes to zoom to 
	/// </summary>
	public float[] zoomLevels;

	/// <summary>
	/// Array of times for each pan
	/// </summary>
	public float[] panTimes;

	/// <summary>
	/// Whether to use ease when panning
	/// </summary>
	public bool useEase;

	/// <summary>
	/// Initializes the pan with the array of pan actions
	/// </summary>
	public override void InitialElement()
	{

		// The pan movement and zoom actions are initialized to default
		movement = new ActionMovement();
		zoomTransition = new ActionZoom();

		// Loops through each pan action
		for (int i = 0;
			i < Mathf.Min(positions.Length, zoomLevels.Length, panTimes.Length);
			i++)
		{

			// Progression decides whether to use ease or not
			IProgression panProgression;
			if (useEase)
			{
				panProgression = new EaseInOut(panTimes[i] / 2, panTimes[i]);
			}
			else
			{
				panProgression = new ConstantProgression(panTimes[i]);
			}

			// Add the pan action for the element
			// with the correct position and zoom
			movement.AddAction(
				new LinearPath(positions[i], positions[i + 1]),
				panProgression);
			zoomTransition.AddAction(
				new Vector2(zoomLevels[i], zoomLevels[i]),
				new Vector2(zoomLevels[i + 1], zoomLevels[i + 1]),
				new ConstantProgression(panTimes[i]));
		}
	}

}
