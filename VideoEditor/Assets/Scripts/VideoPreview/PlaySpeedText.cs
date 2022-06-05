using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaySpeedText : MonoBehaviour
{

	TextMeshProUGUI text;

	BoxCollider2D boxCollider;

	public PreviewSpeed previewSpeed;

	Color blue = new Color(100f / 255f, 149f / 255f, 237f / 255f);

	Color black = Color.black;

	bool textSelected = false;

	bool PeriodUsed
	{
		get
		{
			return speed.Contains(".");
		}
	}

	int TextCharacters
	{
		get
		{
			return speed.Replace(".", "").Length;
		}
	}

	string speed = "2";

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed.ToString("0.00");
		if (newSpeed >= 100)
		{
			speed = speed.Substring(0, 3);
		}
		else
		{
			speed = speed.Substring(0, 4);
		}
		text.text = speed + "x\nspeed";
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
					SpeedEntered();
				}
			}
			else
			{

				text.color = textSelected ? black : blue;
				textSelected = !textSelected;
				SpeedEntered();
			}
		}

		if (textSelected)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				text.color = black;
				textSelected = false;
				SpeedEntered();
			}

			for (int i = 0; i < 10; i++)
			{
				if (Input.GetKeyDown(i.ToString()))
				{
					KeyPressed(i.ToString());
					break;
				}
			}

			if (Input.GetKeyDown(KeyCode.Period))
			{
				KeyPressed(".");
			}

			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				KeyPressed("Backspace");
			}
		}

	}

	private void SpeedEntered()
	{
		if (speed[speed.Length - 1] == '.')
		{
			speed = speed.Replace(".", "");
			text.text = speed + "x\nspeed";
		}

		if (float.Parse(speed) == 0f)
		{
			speed = "2";
			text.text = speed + "x\nspeed";
		}
		previewSpeed.SetSpeed(float.Parse(speed));
	}

	private void KeyPressed(string key)
	{
		if (key.Equals(".") && !PeriodUsed && TextCharacters < 3)
		{
			speed += ".";
		}
		else if (key.Equals("Backspace"))
		{
			if (speed.Length == 1)
			{
				speed = "0";
			}
			else
			{
				speed = speed.Substring(0, speed.Length - 1);
			}
		}
		else if ("0123456789".Contains(key))
		{
			if (TextCharacters < 3)
			{
				if (speed.Equals("0"))
				{
					speed = key;
				}
				else
				{
					speed += key;
				}
			}
			else
			{
				int periodPosition = speed.IndexOf(".");

				speed = speed.Replace(".", "");


				speed += key;

				speed = speed.Substring(1, speed.Length - 1);

				if (periodPosition != -1)
				{
					speed = speed.Insert(periodPosition, ".");
				}

			}
		}

		text.text = speed + "x\nspeed";

	}

}
