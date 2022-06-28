using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Button that pauses/plays the action
/// </summary>
public class PlayPause : MonoBehaviour
{

	/// <summary>
	/// The video preview GUI manager
	/// </summary>
	public VideoPreviewSettings videoPreviewSettings;

	/// <summary>
	/// The sprite for the deselected play button
	/// </summary>
	public Sprite play;

	/// <summary>
	/// The sprite for the deselected pause button
	/// </summary>
	public Sprite pause;

	/// <summary>
	/// The sprite for the selected play button
	/// </summary>
	public Sprite playSelected;

	/// <summary>
	/// The sprite for the selected pause button
	/// </summary>
	public Sprite pauseSelected;

	/// <summary>
	/// The sprite renderer for this button
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
	/// Whether the button is currently on play mode (showing pause button)
	/// </summary>
	private bool onPlayMode;

	/// <summary>
	/// Deselects the button by its button type (play/pause)
	/// </summary>
	public void Deselect()
	{
		if (onPlayMode)
		{
			SpriteRenderer.sprite = pause;
		}
		else
		{
			SpriteRenderer.sprite = play;
		}
	}

	/// <summary>
	/// Selects the button by its button type (play/pause)
	/// </summary>
	public void Select()
	{
		if (onPlayMode)
		{
			SpriteRenderer.sprite = pauseSelected;
		}
		else
		{
			SpriteRenderer.sprite = playSelected;
		}
	}

	/// <summary>
	/// Sets the button to pause state (showing play button)
	/// </summary>
	public void ForcePause()
	{
		onPlayMode = false;
		SpriteRenderer.sprite = play;
	}

	/// <summary>
	/// Sets the button to play state (showing pause button)
	/// </summary>
	public void ForcePlay()
	{
		onPlayMode = true;
		SpriteRenderer.sprite = pause;
	}

	/// <summary>
	/// When the play/pause button is clicked, toggle the pause/play state, and
	/// tell the video preview to play/pause according to the current state
	/// </summary>
	private void OnMouseDown()
	{
		onPlayMode = !onPlayMode;
		if (onPlayMode)
		{
			SpriteRenderer.sprite = pause;
			videoPreviewSettings.Play();
		}
		else
		{
			SpriteRenderer.sprite = play;
			videoPreviewSettings.Pause();
		}
	}

}