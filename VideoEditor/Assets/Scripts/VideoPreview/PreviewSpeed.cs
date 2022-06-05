using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSpeed : MonoBehaviour
{
	public VideoPreviewSettings videoPreviewSettings;

	public PlaySpeedText playSpeedText;

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

	public Sprite selectedSprite;

	public Sprite deselectedSprite;

	public bool positiveSpeed;

	public float speed = 2f;

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
		videoPreviewSettings.UpdatePlaySpeed(newSpeed);
	}

	public void SetTextSpeed(float newSpeed)
	{
		playSpeedText.SetSpeed(newSpeed);
		speed = newSpeed;
	}

	public void Deselect()
	{
		SpriteRenderer.sprite = deselectedSprite;
	}

	public void Select()
	{
		SpriteRenderer.sprite = selectedSprite;
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnMouseDown()
	{
		videoPreviewSettings.SetPlaySpeed(speed * (positiveSpeed ? 1 : -1));
	}
}
