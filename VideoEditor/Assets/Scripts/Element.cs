using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{

	protected ActionMovement movement;

	protected ActionColor colorTransition;

	protected ActionZoom zoomTransition;

	protected ActionRotate rotateTransition;

	private GameObject ParentB
	{
		get
		{
			if (movement != null && zoomTransition != null && rotateTransition != null)
			{
				if (_parentB == null)
				{
					_parentB = new GameObject();
					_parentB.transform.localPosition = new Vector2(0, 0);
					_parentB.transform.name = name + "ParentB";
				}
				return _parentB;
			}
			return null;
		}
		set
		{
			_parentB = value;
		}
	}
	private GameObject _parentB;

	private GameObject ParentA
	{
		get
		{
			if ((movement != null && zoomTransition != null) ||
				(movement != null && rotateTransition != null) ||
				(zoomTransition != null && rotateTransition != null))
			{
				if (_parentA == null)
				{
					_parentA = new GameObject();
					_parentA.transform.localPosition = new Vector2(0, 0);
					_parentA.transform.name = name + "ParentA";

					Transform gameObjectParent = gameObject.transform.parent;
					if (ParentB != null)
					{
						transform.parent = ParentB.transform;
						ParentB.transform.parent = _parentA.transform;
						_parentA.transform.parent = gameObjectParent;
					}
					else
					{

						transform.parent = ParentA.transform;
						_parentA.transform.parent = gameObjectParent;
					}
				}
				return _parentA;
			}
			return null;
		}
		set
		{
			_parentA = value;
		}
	}
	private GameObject _parentA;

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

	protected TransformOrder transformOrder;

	public abstract void InitialElement();

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

		if (zoomTransition != null)
		{
			transform.localScale = zoomTransition.GetElementSize(0f);
		}
		else
		{
			transform.localScale = new Vector2(1, 1);
		}

		if (rotateTransition != null)
		{
			Quaternion rotationQuaternion = new Quaternion();
			rotationQuaternion.eulerAngles = new Vector3(0, 0, rotateTransition.GetElementRotation(0f));
			transform.localRotation = rotationQuaternion;
		}
		else
		{
			Quaternion rotationQuaternion = new Quaternion();
			rotationQuaternion.eulerAngles = new Vector3(0, 0, 0);
			transform.localRotation = rotationQuaternion;
		}

		if (colorTransition != null)
		{
			SpriteRenderer.color = colorTransition.GetElementColor(0f);
		}
		else
		{
			SpriteRenderer.color = Color.white;
		}
	}


	public void UpdateElement(float actionTime)
	{

		Transform move = null;
		Transform zoom = null;
		Transform rotate = null;

		Transform zoomChild = null;
		Transform rotateChild = null;

		if (ParentB != null)
		{
			if (transformOrder == TransformOrder.MoveRotateZoom || transformOrder == TransformOrder.MoveZoomRotate)
			{
				move = ParentA.transform;
			}
			else if (transformOrder == TransformOrder.ZoomMoveRotate || transformOrder == TransformOrder.RotateMoveZoom)
			{
				move = ParentB.transform;
			}
			else
			{
				move = transform;
			}

			if (transformOrder == TransformOrder.ZoomMoveRotate || transformOrder == TransformOrder.ZoomRotateMove)
			{
				zoom = ParentA.transform;
				zoomChild = ParentB.transform;
			}
			else if (transformOrder == TransformOrder.MoveZoomRotate || transformOrder == TransformOrder.RotateZoomMove)
			{
				zoom = ParentB.transform;
				zoomChild = transform;
			}
			else
			{
				zoom = transform;
			}

			if (transformOrder == TransformOrder.RotateMoveZoom || transformOrder == TransformOrder.RotateZoomMove)
			{
				rotate = ParentA.transform;
				rotateChild = ParentB.transform;
			}
			else if (transformOrder == TransformOrder.MoveRotateZoom || transformOrder == TransformOrder.ZoomRotateMove)
			{
				rotate = ParentB.transform;
				rotateChild = transform;
			}
			else
			{
				rotate = transform;
			}
		}
		else if (ParentA != null)
		{
			if (movement == null)
			{
				if (transformOrder == TransformOrder.RotateMoveZoom ||
					transformOrder == TransformOrder.RotateZoomMove ||
					transformOrder == TransformOrder.MoveRotateZoom)
				{
					rotate = ParentA.transform;
					rotateChild = transform;

					zoom = transform;
				}
				else
				{
					zoom = ParentA.transform;
					zoomChild = transform;

					rotate = transform;
				}
			}
			else if (zoomTransition == null)
			{
				if (transformOrder == TransformOrder.ZoomMoveRotate ||
					transformOrder == TransformOrder.MoveRotateZoom ||
					transformOrder == TransformOrder.MoveZoomRotate)
				{
					move = ParentA.transform;

					rotate = transform;
				}
				else
				{
					rotate = ParentA.transform;
					rotateChild = transform;

					move = transform;
				}
			}
			else if (rotateTransition == null)
			{
				if (transformOrder == TransformOrder.ZoomMoveRotate ||
					transformOrder == TransformOrder.ZoomRotateMove ||
					transformOrder == TransformOrder.RotateZoomMove)
				{
					zoom = ParentA.transform;
					zoomChild = transform;

					move = transform;
				}
				else
				{
					zoom = ParentA.transform;

					move = transform;
				}
			}
		}
		else
		{
			if (movement != null)
			{
				move = transform;
			}
			else if (zoomTransition != null)
			{
				zoom = transform;
			}
			else if (rotateTransition != null)
			{
				rotate = transform;
			}
		}

		if (move != null)
		{
			move.localPosition = movement.GetElementPosition(actionTime);
			if (zoomChild != null && move.GetInstanceID() == zoomChild.GetInstanceID())
			{
				Vector2 pivotPosition = zoomTransition.GetPivotPosition(actionTime);
				move.localPosition += new Vector3(pivotPosition.x, pivotPosition.y, 0);
			}
			else if (rotateChild != null && move.GetInstanceID() == rotateChild.GetInstanceID())
			{
				Vector2 pivotPosition = rotateTransition.GetPivotPosition(actionTime);
				move.localPosition += new Vector3(pivotPosition.x, pivotPosition.y, 0);
			}
		}

		if (zoom != null)
		{
			zoom.localScale = zoomTransition.GetElementSize(actionTime);
			zoom.localPosition = -zoomTransition.GetPivotPosition(actionTime);
			if (rotateChild != null && zoom.GetInstanceID() == rotateChild.GetInstanceID())
			{
				Vector2 pivotPosition = rotateTransition.GetPivotPosition(actionTime);
				zoom.localPosition = new Vector3(pivotPosition.x, pivotPosition.y, 0);
			}
		}

		if (rotate != null)
		{
			Quaternion rotationQuaternion = new Quaternion();
			rotationQuaternion.eulerAngles = new Vector3(0, 0, rotateTransition.GetElementRotation(actionTime));
			rotate.localRotation = rotationQuaternion;
			rotate.localPosition = -rotateTransition.GetPivotPosition(actionTime);
			if (zoomChild != null && rotate.GetInstanceID() == zoomChild.GetInstanceID())
			{
				Vector2 pivotPosition = zoomTransition.GetPivotPosition(actionTime);
				rotate.localPosition = new Vector3(pivotPosition.x, pivotPosition.y, 0);
			}
		}

		if (colorTransition != null)
		{
			SpriteRenderer.color = colorTransition.GetElementColor(actionTime);
		}

	}


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
		if (rotateTransition != null)
		{
			maxTime = Mathf.Max(maxTime, rotateTransition.TotalActionTime());
		}
		return maxTime;
	}

}

public enum TransformOrder
{
	MoveRotateZoom,
	MoveZoomRotate,
	RotateZoomMove,
	RotateMoveZoom,
	ZoomRotateMove,
	ZoomMoveRotate
}