using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StepText : MonoBehaviour
{
	TextMeshProUGUI text;

	BoxCollider2D boxCollider;

	public PreviewSkip previewSkip;

	Color blue = new Color(100f / 255f, 149f / 255f, 237f / 255f);

	Color black = Color.black;

	bool textSelected = false;


	string frames = "10";

	public void SetFrames(int newFrames)
	{
		frames = newFrames.ToString();
		text.text = frames + "x\nframes";
	}

	// Start is called before the first frame update
	void Start()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{

			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0;
			if (!boxCollider.bounds.Contains(mousePosition))
			{
				if (textSelected)
				{
					text.color = black;
					textSelected = false;
					FramesEntered();
				}
			}
			else
			{

				text.color = textSelected ? black : blue;
				textSelected = !textSelected;
				FramesEntered();
			}
		}

		if (textSelected)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				text.color = black;
				textSelected = false;
				FramesEntered();
			}

			for (int i = 0; i < 10; i++)
			{
				if (Input.GetKeyDown(i.ToString()))
				{
					KeyPressed(i.ToString());
					break;
				}
			}

			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				KeyPressed("Backspace");
			}
		}

	}

	private void FramesEntered()
	{
		if (int.Parse(frames) == 0)
		{
			frames = "10";
			text.text = frames + "x\nframes";
		}
		previewSkip.SetFrames(int.Parse(frames));
	}

	private void KeyPressed(string key)
	{
		if (key.Equals("Backspace"))
		{
			if (frames.Length == 1)
			{
				frames = "0";
			}
			else
			{
				frames = frames.Substring(0, frames.Length - 1);
			}
		}
		else if ("0123456789".Contains(key))
		{
			if (frames.Length < 3)
			{
				if (frames.Equals("0"))
				{
					frames = key;
				}
				else
				{
					frames += key;
				}
			}
			else
			{
				frames += key;
				frames = frames.Substring(1, frames.Length - 1);
			}
		}

		text.text = frames + "x\nframes";

	}
}
