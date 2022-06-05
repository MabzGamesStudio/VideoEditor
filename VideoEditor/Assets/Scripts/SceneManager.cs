using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

	public Element circle1;
	public Element circle2;

	private ActionManager actionManager;

	// Start is called before the first frame update
	void Start()
	{
		actionManager = GetComponent<ActionManager>();

		List<Element> elements = new List<Element>();
		elements.Add(circle1);
		elements.Add(circle2);

		actionManager.SetElements(elements);

	}

	// Update is called once per frame
	void Update()
	{

	}

}
