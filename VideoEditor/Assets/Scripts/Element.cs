using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{

	protected ActionMovement movement;

	protected ActionColor colorTransition;

	protected ActionZoom zoomTransition;

	private int zoomParentID = -1;

	private GameObject ZoomParent
	{
		get
		{
			if (zoomParentID == -1)
			{
				_zoomParent = new GameObject();
				_zoomParent.transform.localPosition = new Vector2(0, 0);
				_zoomParent.name = gameObject.name + "ZoomParent";
				if (transform.parent != null)
				{
					gameObject.transform.parent = _zoomParent.transform;
				}
				gameObject.transform.parent = _zoomParent.transform;
				zoomParentID = _zoomParent.GetInstanceID();
			}
			return _zoomParent;
		}
	}
	private GameObject _zoomParent;

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
		if (movement != null)
		{
			transform.localPosition = movement.GetElementPosition(0f);
		}
		else
		{
			transform.localPosition = new Vector2(0, 0);
		}
	}

	public void UpdateElement(float actionTime)
	{

		Vector2 zoomTransform = new Vector2(0, 0);
		if (zoomTransform != null)
		{
			zoomTransform = zoomTransition.GetPivotPosition(actionTime);
			ZoomParent.transform.localPosition = zoomTransform;
		}

		if (movement != null)
		{
			if (zoomTransition != null)
			{
				ZoomParent.transform.localPosition = movement.GetElementPosition(actionTime) + zoomTransform;
				transform.localPosition = -zoomTransform;
			}
			else
			{
				transform.localPosition = movement.GetElementPosition(actionTime);
			}
		}

		if (colorTransition != null)
		{
			SpriteRenderer.color = colorTransition.GetElementColor(actionTime);
		}

		if (zoomTransition != null)
		{
			ZoomParent.transform.localScale = zoomTransition.GetElementSize(actionTime);
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
		if (zoomTransition != null)
		{
			maxTime = Mathf.Max(maxTime, zoomTransition.TotalActionTime());
		}
		return maxTime;
	}

}
