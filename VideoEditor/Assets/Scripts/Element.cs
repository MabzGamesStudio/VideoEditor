using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{

	protected ActionMovement movement;

	protected ActionColor colorTransition;

	private SpriteRenderer SpriteRenderer
	{
		get
		{
			if (_spriteRenderer == null)
			{
				_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			}
			return _spriteRenderer;
		}
		set
		{
			_spriteRenderer = value;
		}
	}
	private SpriteRenderer _spriteRenderer;

	// Start is called before the first frame update
	void Start()
	{
		InitialElement();
		transform.localPosition = movement.GetElementPosition(0f);
	}

	public void UpdateElement(float actionTime)
	{
		if (movement != null)
		{
			transform.localPosition = movement.GetElementPosition(actionTime);
		}
		if (colorTransition != null)
		{
			SpriteRenderer.color = colorTransition.GetElementColor(actionTime);
		}
	}

	public abstract void InitialElement();

	public float GetTotalActionTime()
	{
		float maxTime = 0;
		if (movement != null)
		{
			maxTime = Mathf.Max(maxTime, movement.TotalActionTime());
		}
		if (colorTransition != null)
		{
			maxTime = Mathf.Max(maxTime, colorTransition.TotalActionTime());
		}
		return maxTime;
	}

}
