using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A game object that is controlled by actions
/// </summary>
public abstract class Element : MonoBehaviour
{

	/// <summary>
	/// Movement of the element
	/// </summary>
	protected ActionMovement movement;

	/// <summary>
	/// Color of the element
	/// </summary>
	protected ActionColor colorTransition;

	/// <summary>
	/// Scale of the element
	/// </summary>
	protected ActionZoom zoomTransition;

	/// <summary>
	/// Rotation of the element
	/// </summary>
	protected ActionRotate rotateTransition;

	/// <summary>
	/// Whether the element has been initialized
	/// </summary>
	private bool elementInitialized;

	/// <summary>
	/// Whether the Action Color is for the sprite or text mesh
	/// </summary>
	public bool usingTextMesh;

	/// <summary>
	/// The number of transformation transitions used
	/// </summary>
	private int TransformationNumber
	{
		get
		{

			// Count the number of transformation transitions used
			if (_transformationNumber == -1)
			{
				int count = 0;
				if (movement != null)
				{
					count++;
				}
				if (zoomTransition != null)
				{
					count++;
				}
				if (rotateTransition != null)
				{
					count++;
				}

				// Set the transformation number
				_transformationNumber = count;
			}

			// Return computed transformation number
			return _transformationNumber;
		}
		set
		{
			_transformationNumber = value;
		}
	}
	private int _transformationNumber = -1;

	/// <summary>
	/// The primary parent of the element to allow rotation/scaling from any
	/// pivot position
	/// </summary>
	private GameObject ParentA
	{
		get
		{

			// If there is at least two non-null transformations
			if (TransformationNumber >= 1)
			{
				if (_parentA == null)
				{

					// Create parent a game object
					_parentA = new GameObject();
					_parentA.transform.localPosition = new Vector2(0, 0);
					_parentA.transform.name = name + "ParentA";

					// Set parents hierarchy
					Transform gameObjectParent = gameObject.transform.parent;
					if (ParentB != null)
					{
						if (ParentC != null)
						{
							transform.parent = ParentC.transform;
							ParentC.transform.parent = ParentB.transform;
							ParentB.transform.parent = _parentA.transform;
							_parentA.transform.parent = gameObjectParent;
						}
						else
						{
							transform.parent = ParentB.transform;
							ParentB.transform.parent = _parentA.transform;
							_parentA.transform.parent = gameObjectParent;
						}
					}
					else
					{
						transform.parent = _parentA.transform;
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

	/// <summary>
	/// The secondary parent of the element to allow rotation/scaling from any
	/// pivot position
	/// </summary>
	private GameObject ParentB
	{
		get
		{

			// If there are all three transformations
			if (TransformationNumber >= 2)
			{

				// Create the parent C if not yet created
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

	/// <summary>
	/// The tertiary parent of the element to allow rotation/scaling from any
	/// pivot position
	/// </summary>
	private GameObject ParentC
	{
		get
		{

			// If there are all three transformations
			if (TransformationNumber == 3)
			{

				// Create the parent C if not yet created
				if (_parentC == null)
				{
					_parentC = new GameObject();
					_parentC.transform.localPosition = new Vector2(0, 0);
					_parentC.transform.name = name + "ParentC";
				}
				return _parentC;
			}
			return null;
		}
		set
		{
			_parentC = value;
		}
	}
	private GameObject _parentC;

	/// <summary>
	/// The sprite renderer of the game object
	/// </summary>
	private SpriteRenderer SpriteRenderer
	{
		get
		{
			if (_spriteRenderer == null)
			{
				_spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
			}
			return _spriteRenderer;
		}
		set
		{
			_spriteRenderer = value;
		}
	}
	private SpriteRenderer _spriteRenderer;

	/// <summary>
	/// The text mesh of the game object
	/// </summary>
	private TextMeshProUGUI TextMesh
	{
		get
		{
			if (_textMesh == null)
			{
				_textMesh = gameObject.GetComponentInChildren<TextMeshProUGUI>();
			}
			return _textMesh;
		}
		set
		{
			_textMesh = value;
		}
	}
	private TextMeshProUGUI _textMesh;

	/// <summary>
	/// The order for the scale, rotate, and movement transformation
	/// </summary>
	protected TransformOrder transformOrder;

	/// <summary>
	/// This initializes the element actions
	/// </summary>
	public abstract void InitialElement();


	/// <summary>
	/// Initializes the element and the actions of the element
	/// </summary>
	private void Start()
	{
		TryElementInit();
		ActionInit();
	}

	/// <summary>
	/// Initializes the movement, zoom, rotation, and movement actions
	/// </summary>
	private void ActionInit()
	{

		// Movement init
		if (movement != null)
		{
			transform.localPosition = movement.GetElementPosition(0f);
		}
		else
		{
			transform.localPosition = new Vector2(0, 0);
		}

		// Zoom init
		if (zoomTransition != null)
		{
			transform.localScale = zoomTransition.GetElementSize(0f);
		}
		else
		{
			transform.localScale = new Vector2(1, 1);
		}

		// Rotation init
		if (rotateTransition != null)
		{
			Quaternion rotationQuaternion = new Quaternion();
			rotationQuaternion.eulerAngles =
				new Vector3(0, 0, rotateTransition.GetElementRotation(0f));
			transform.localRotation = rotationQuaternion;
		}
		else
		{
			Quaternion rotationQuaternion = new Quaternion();
			rotationQuaternion.eulerAngles = new Vector3(0, 0, 0);
			transform.localRotation = rotationQuaternion;
		}

		// Color init
		if (colorTransition != null)
		{
			if (usingTextMesh)
			{
				TextMesh.color = colorTransition.GetElementColor(0f);
			}
			else
			{
				SpriteRenderer.color = colorTransition.GetElementColor(0f);
			}
		}
		else
		{
			if (usingTextMesh)
			{
				TextMesh.color = Color.white;
			}
			else
			{
				SpriteRenderer.color = Color.white;
			}
		}
	}

	/// <summary>
	/// Initializes element if not yet already initialized
	/// </summary>
	private void TryElementInit()
	{
		if (!elementInitialized)
		{
			InitialElement();
			elementInitialized = true;
		}
	}

	/// <summary>
	/// Updates the element action based on the action time
	/// </summary>
	/// <param name="actionTime">Time progression of the action</param>
	public void UpdateElement(float actionTime)
	{

		// move, zoom, and rotate element parents are init to null
		Transform move = null;
		Transform zoom = null;
		Transform rotate = null;

		// Child transforms of the zoom and rotate
		Transform zoomChild = null;
		Transform rotateChild = null;

		// There are three transformations
		if (TransformationNumber == 3)
		{

			// Move first
			if (transformOrder == TransformOrder.MoveRotateZoom ||
				transformOrder == TransformOrder.MoveZoomRotate)
			{
				move = ParentA.transform;
			}

			// Move second
			else if (transformOrder == TransformOrder.ZoomMoveRotate ||
				transformOrder == TransformOrder.RotateMoveZoom)
			{
				move = ParentB.transform;
			}

			// Move last
			else
			{
				move = ParentC.transform;
			}

			// Zoom first
			if (transformOrder == TransformOrder.ZoomMoveRotate ||
				transformOrder == TransformOrder.ZoomRotateMove)
			{
				zoom = ParentA.transform;
				zoomChild = ParentB.transform;
			}

			// Zoom second
			else if (transformOrder == TransformOrder.MoveZoomRotate ||
				transformOrder == TransformOrder.RotateZoomMove)
			{
				zoom = ParentB.transform;
				zoomChild = ParentC.transform;
			}

			// Zoom last
			else
			{
				zoom = ParentC.transform;
				zoomChild = transform;
			}

			// Rotate first
			if (transformOrder == TransformOrder.RotateMoveZoom ||
				transformOrder == TransformOrder.RotateZoomMove)
			{
				rotate = ParentA.transform;
				rotateChild = ParentB.transform;
			}

			// Rotate second
			else if (transformOrder == TransformOrder.MoveRotateZoom ||
				transformOrder == TransformOrder.ZoomRotateMove)
			{
				rotate = ParentB.transform;
				rotateChild = ParentC.transform;
			}

			// Rotate last
			else
			{
				rotate = ParentC.transform;
				rotateChild = transform;
			}
		}

		// There are two transformations
		else if (TransformationNumber == 2)
		{

			// No movement
			if (movement == null)
			{

				// Rotate before zoom
				if (transformOrder == TransformOrder.RotateMoveZoom ||
					transformOrder == TransformOrder.RotateZoomMove ||
					transformOrder == TransformOrder.MoveRotateZoom)
				{
					rotate = ParentA.transform;
					rotateChild = ParentB.transform;
					zoom = ParentB.transform;
					zoomChild = transform;
				}

				// Zoom before rotate
				else
				{
					zoom = ParentA.transform;
					zoomChild = ParentB.transform;
					rotate = ParentB.transform;
					rotateChild = transform;
				}
			}

			// No zoom
			else if (zoomTransition == null)
			{

				// Move before rotate
				if (transformOrder == TransformOrder.ZoomMoveRotate ||
					transformOrder == TransformOrder.MoveRotateZoom ||
					transformOrder == TransformOrder.MoveZoomRotate)
				{
					move = ParentA.transform;
					rotate = ParentB.transform;
					rotateChild = transform;
				}

				// Rotate before move
				else
				{
					rotate = ParentA.transform;
					rotateChild = ParentB.transform;
					move = ParentB.transform;
				}
			}

			// No rotation
			else if (rotateTransition == null)
			{

				// Zoom before move
				if (transformOrder == TransformOrder.ZoomMoveRotate ||
					transformOrder == TransformOrder.ZoomRotateMove ||
					transformOrder == TransformOrder.RotateZoomMove)
				{
					zoom = ParentA.transform;
					zoomChild = ParentB.transform;
					move = ParentB.transform;
				}

				// Move before zoom
				else
				{
					move = ParentA.transform;
					zoom = ParentB.transform;
					zoomChild = transform;
				}
			}
		}

		// There is one transformation
		else if (TransformationNumber == 1)
		{
			if (movement != null)
			{
				move = ParentA.transform;
			}
			else if (zoomTransition != null)
			{
				zoom = ParentA.transform;
				zoomChild = transform;
			}
			else if (rotateTransition != null)
			{
				rotate = ParentA.transform;
				rotateChild = transform;
			}
		}

		// TODO: Make transformations work!!!
		// If there is a move action move the correct transformation and fix any
		// problems with the pivot
		if (move != null)
		{

			// Move the correct move transformation
			move.localPosition = movement.GetElementPosition(actionTime);

			// If the move transformation is the child of the zoom,
			// then offset the position by the zoom pivot
			if (zoomChild != null && move.GetInstanceID() == zoomChild.GetInstanceID())
			{
				Vector2 pivotPosition = zoomTransition.GetPivotPosition(actionTime);
				Vector2 pivotZoom = zoomTransition.GetElementSize(actionTime);
				zoomChild.localPosition -= new Vector3(pivotPosition.x, pivotPosition.y, 0);
			}

			// If the move transformation is the child of the rotate,
			// then offset the position by the rotate pivot
			else if (rotateChild != null && move.GetInstanceID() == rotateChild.GetInstanceID())
			{
				Vector2 pivotPosition = rotateTransition.GetPivotPosition(actionTime);
				move.localPosition -= new Vector3(pivotPosition.x, pivotPosition.y, 0);
			}
		}

		// If there is a zoom action move the correct transformation and fix any
		// problems with the pivot
		if (zoom != null)
		{

			// Set scale to zoom
			zoom.localScale = zoomTransition.GetElementSize(actionTime);

			// Move zoom transform by zoom pivot
			zoom.localPosition = zoomTransition.GetPivotPosition(actionTime);

			// If the zoom transformation is the child of the rotate,
			// then offset the position by the rotate pivot
			if (rotateChild != null && zoom.GetInstanceID() == rotateChild.GetInstanceID())
			{
				Vector2 pivotPosition = rotateTransition.GetPivotPosition(actionTime);
				zoom.localPosition -= new Vector3(pivotPosition.x, pivotPosition.y, 0);
			}
		}

		// If there is a rotate action move the correct transformation and fix any
		// problems with the pivot
		if (rotate != null)
		{

			// Set rotation (its a simple rotation but need to use quaternions)
			Quaternion rotationQuaternion = new Quaternion();
			rotationQuaternion.eulerAngles = new Vector3(0, 0, rotateTransition.GetElementRotation(actionTime));
			rotate.localRotation = rotationQuaternion;

			// Move rotate transformation by rotate pivot
			rotate.localPosition = rotateTransition.GetPivotPosition(actionTime);

			// If the rotate transformation is the child of the zoom,
			// then offset the position by the zoom pivot
			if (zoomChild != null && rotate.GetInstanceID() == zoomChild.GetInstanceID())
			{
				Vector2 pivotPosition = zoomTransition.GetPivotPosition(actionTime);
				Vector2 pivotZoom = zoomTransition.GetElementSize(actionTime);
				rotate.localPosition -= new Vector3(pivotPosition.x, pivotPosition.y, 0);
			}
		}

		// If the element transformation is the child of the zoom,
		// then offset the position by the zoom pivot
		if (zoomChild != null && transform.GetInstanceID() == zoomChild.GetInstanceID())
		{
			Vector2 pivotPosition = zoomTransition.GetPivotPosition(actionTime);
			Vector2 pivotZoom = zoomTransition.GetElementSize(actionTime);
			transform.localPosition = -new Vector3(pivotPosition.x, pivotPosition.y, 0);
		}

		// If the element transformation is the child of the rotate,
		// then offset the position by the rotate pivot
		if (rotateChild != null && transform.GetInstanceID() == rotateChild.GetInstanceID())
		{
			Vector2 pivotPosition = rotateTransition.GetPivotPosition(actionTime);
			transform.localPosition = -new Vector3(pivotPosition.x, pivotPosition.y, 0);
		}

		// If there is a color action, then set it
		if (colorTransition != null)
		{
			if (usingTextMesh)
			{
				TextMesh.color = colorTransition.GetElementColor(actionTime);
			}
			else
			{
				SpriteRenderer.color = colorTransition.GetElementColor(actionTime);
			}
		}
	}

	/// <summary>
	/// Gets the total time of the longest element action
	/// </summary>
	/// <returns>Total time for action</returns>
	public float GetTotalActionTime()
	{

		// Initialize element if not already initialized
		if (!elementInitialized)
		{
			InitialElement();
			elementInitialized = true;
		}

		// Max value of all actions is init to 0
		float maxTime = 0;

		// Get max action time of move, color, zoom, and rotate
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

		// Return the max of the action times
		return maxTime;
	}

}

/// <summary>
/// The order of the move, rotate, and zoom transformations
/// </summary>
public enum TransformOrder
{
	MoveRotateZoom,
	MoveZoomRotate,
	RotateZoomMove,
	RotateMoveZoom,
	ZoomRotateMove,
	ZoomMoveRotate
}