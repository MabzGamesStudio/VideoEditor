using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slider progress knob for the preview action
/// </summary>
public class PreviewSlider : MonoBehaviour
{

	/// <summary>
	/// The world width of the slider
	/// </summary>
	public float width;

	/// <summary>
	/// The current position of the knob
	/// </summary>
	public float currentPosition;

	/// <summary>
	/// The video preview GUI manager
	/// </summary>
	public VideoPreviewSettings videoPreviewSettings;

	/// <summary>
	/// On start of the game, set the knob to the start position at the left
	/// </summary>
	private void Start()
	{
		transform.localPosition = new Vector2(-(width / 2),
			transform.localPosition.y);
	}

	/// <summary>
	/// Updates the slider to a position based on progress input
	/// </summary>
	/// <param name="videoProgression">Progress from 0 to 1</param>
	public void UpdateSlider(float videoProgression)
	{
		transform.localPosition = new Vector2(videoProgression * width -
			(width / 2), transform.localPosition.y);
	}

	/// <summary>
	/// When the circle is dragged, update the knob position on the slider
	/// </summary>
	private void OnMouseDrag()
	{

		// Set the position to the mouse world position
		float mouseXPosition =
			Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

		// Keep knob position within the bounds of the slider
		float knobPosition = Mathf.Min(width / 2, mouseXPosition);
		knobPosition = Mathf.Max(-width / 2, knobPosition);

		// Set knob transform position
		transform.localPosition =
			new Vector2(knobPosition, transform.localPosition.y);

		// Tell video preview the current new progress
		videoPreviewSettings.SetActionTime(knobPosition / width + .5f);
	}

}
