using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPause : MonoBehaviour
{


	public VideoPreviewSettings videoPreviewSettings;

	public Sprite play;

	public Sprite pause;

	public Sprite playSelected;

	public Sprite pauseSelected;

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

	private bool onPlayMode;



	// Start is called before the first frame update
	void Start()
	{

	}

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

	public void ForcePause()
	{
		onPlayMode = false;
		SpriteRenderer.sprite = play;
	}

	public void ForcePlay()
	{
		onPlayMode = true;
		SpriteRenderer.sprite = pause;
	}

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
