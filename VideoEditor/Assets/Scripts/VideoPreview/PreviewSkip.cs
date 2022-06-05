using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSkip : MonoBehaviour
{

	public VideoPreviewSettings videoPreviewSettings;

	public StepText previewSkipText;

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

	public Sprite deselectedSprite;

	public Sprite selectedSprite;

	public bool forwardSkip;

	public int skipNumber = 10;

	// Update is called once per frame
	void Update()
	{

	}

	public void Deselect()
	{
		SpriteRenderer.sprite = deselectedSprite;
	}

	public void Select()
	{
		SpriteRenderer.sprite = selectedSprite;
	}

	public void SetFrames(int newFrames)
	{
		videoPreviewSettings.UpdateSkipFrames(newFrames);
		skipNumber = newFrames;
	}

	public void SetTextFrames(int newFrames)
	{
		previewSkipText.SetFrames(newFrames);
		skipNumber = newFrames;
	}

	private void OnMouseDown()
	{
		videoPreviewSettings.StepPreview(skipNumber * (forwardSkip ? 1 : -1));
	}
}
