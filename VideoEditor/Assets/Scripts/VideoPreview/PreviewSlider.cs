using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSlider : MonoBehaviour
{

	public float width;

	public float currentPosition;

	public VideoPreviewSettings videoPreviewSettings;

	private void Start()
	{
		transform.localPosition = new Vector2(-(width / 2), transform.localPosition.y);
	}

	public void UpdateSlider(float videoProgression)
	{
		transform.localPosition = new Vector2(videoProgression * width - (width / 2), transform.localPosition.y);
	}

	private void OnMouseDrag()
	{
		float mouseXPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
		float knobPosition = Mathf.Min(width / 2, mouseXPosition);
		knobPosition = Mathf.Max(-width / 2, knobPosition);
		transform.localPosition = new Vector2(knobPosition, transform.localPosition.y);
		videoPreviewSettings.SetActionTime(knobPosition / width + .5f);
	}
}
