using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript : MonoBehaviour
{

	private Action action;

	private bool actionGoing;
	private float actionTime;

	// Start is called before the first frame update
	void Start()
	{

		Path path1 = new StillPath(new Vector2(0, 0));
		Path path2 = new LinearPath(new Vector2(0, 0), new Vector2(3, 1));

		Movement movement1 = new ConstantMovement(1);
		Movement movement2 = new ConstantMovement(2);

		action = new Action()
			.AddAction(path1, movement1)
			.AddAction(path2, movement2);
	}

	private void ShowAction()
	{
		actionGoing = true;
		actionTime = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (actionGoing)
		{
			actionTime += Time.deltaTime;
			transform.localPosition = action.GetElementPosition(actionTime);
			if (action.ActionComplete(actionTime))
			{
				actionGoing = false;
			}
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			ShowAction();
		}
	}
}
