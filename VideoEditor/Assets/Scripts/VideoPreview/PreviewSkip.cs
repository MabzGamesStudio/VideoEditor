using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Button for skipping frames of the action
/// </summary>
public class PreviewSkip : MonoBehaviour
{

	/// <summary>
	/// The video preview GUI manager
	/// </summary>
	public VideoPreviewSettings videoPreviewSettings;

	/// <summary>
	/// Text for the skip button
	/// </summary>
	public StepText previewSkipText;

	/// <summary>
	/// Sprite renderer for the button
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
	/// Sprite for when the skip button is not selected
	/// </summary>
	public Sprite deselectedSprite;

	/// <summary>
	/// Sprite for when the skip button is selected
	/// </summary>
	public Sprite selectedSprite;

	/// <summary>
	/// Whether this skip button goes forward
	/// </summary>
	public bool forwardSkip;

	/// <summary>
	/// The number of frames to skip
	/// </summary>
	public int skipNumber = 10;

	/// <summary>
	/// Deselects the skip button
	/// </summary>
	public void Deselect()
	{
		SpriteRenderer.sprite = deselectedSprite;
	}

	/// <summary>
	/// Selects the skip button
	/// </summary>
	public void Select()
	{
		SpriteRenderer.sprite = selectedSprite;
	}

	/// <summary>
	/// Sets the frames per skip button click
	/// </summary>
	/// <param name="newFrames">Number of frames to skip</param>
	public void SetFrames(int newFrames)
	{
		videoPreviewSettings.UpdateSkipFrames(newFrames);
		skipNumber = newFrames;
	}

	/// <summary>
	/// Sets the text for this skip button to the given number
	/// </summary>
	/// <param name="newFrames">Number of frames to skip for display</param>
	public void SetTextFrames(int newFrames)
	{
		previewSkipText.SetFrames(newFrames);
		skipNumber = newFrames;
	}

	/// <summary>
	/// When the button is clicked, tell the video preview to skip a certai
	/// amount of frames of animation forward or backward
	/// </summary>
	private void OnMouseDown()
	{
		videoPreviewSettings.StepPreview(skipNumber * (forwardSkip ? 1 : -1));
	}

}
