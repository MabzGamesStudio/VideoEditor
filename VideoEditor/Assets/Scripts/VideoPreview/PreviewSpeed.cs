using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fast forward/rewind button for the preview action
/// </summary>
public class PreviewSpeed : MonoBehaviour
{

	/// <summary>
	/// The video preview GUI manager
	/// </summary>
	public VideoPreviewSettings videoPreviewSettings;

	/// <summary>
	/// The text for the play speed button to display the speed rate
	/// </summary>
	public PlaySpeedText playSpeedText;

	/// <summary>
	/// The sprite renderer for the button
	/// </summary>
	private SpriteRenderer SpriteRenderer
	{
		get
		{
			if (_spriteRenderer == null)
			{
				_spriteRenderer = GetComponent<SpriteRenderer>();
			}
			return _spriteRenderer;
		}
	}
	private SpriteRenderer _spriteRenderer;

	/// <summary>
	/// The sprite for the selected speed state
	/// </summary>
	public Sprite selectedSprite;

	/// <summary>
	/// The sprite for the deselected speed state
	/// </summary>
	public Sprite deselectedSprite;

	/// <summary>
	/// Whether the button represents a fast forward button
	/// </summary>
	public bool positiveSpeed;

	/// <summary>
	/// The action rate (1 is normal speed)
	/// </summary>
	public float speed = 2f;

	/// <summary>
	/// Sets the speed of the video (1 is normal speed rate, 2 is 2x speed)
	/// </summary>
	/// <param name="newSpeed">Speed of action</param>
	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
		videoPreviewSettings.UpdatePlaySpeed(newSpeed);
	}

	/// <summary>
	/// Sets the text display speed
	/// </summary>
	/// <param name="newSpeed">Speed of the action for display</param>
	public void SetTextSpeed(float newSpeed)
	{
		playSpeedText.SetSpeed(newSpeed);
		speed = newSpeed;
	}

	/// <summary>
	/// Deselects the speed button
	/// </summary>
	public void Deselect()
	{
		SpriteRenderer.sprite = deselectedSprite;
	}

	/// <summary>
	/// Selects the speed button
	/// </summary>
	public void Select()
	{
		SpriteRenderer.sprite = selectedSprite;
	}

	/// <summary>
	/// When the button is clicked, set the action speed to the speed variable
	/// </summary>
	private void OnMouseDown()
	{
		videoPreviewSettings.SetPlaySpeed(speed * (positiveSpeed ? 1 : -1));
	}

}