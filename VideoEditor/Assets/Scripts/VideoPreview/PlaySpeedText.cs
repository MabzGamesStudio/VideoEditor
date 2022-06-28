using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaySpeedText : MonoBehaviour
{

	/// <summary>
	/// The text mesh pro UGUI component
	/// </summary>
	private TextMeshProUGUI text;

	/// <summary>
	/// The box collider for the text selection
	/// </summary>
	private BoxCollider2D boxCollider;

	/// <summary>
	/// The text speed button script
	/// </summary>
	public PreviewSpeed previewSpeed;

	/// <summary>
	/// The blue selected color
	/// </summary>
	Color blue = new Color(100f / 255f, 149f / 255f, 237f / 255f);

	/// <summary>
	/// The black unselected color
	/// </summary>
	Color black = Color.black;

	/// <summary>
	/// Whether the text is currently selected
	/// </summary>
	bool textSelected = false;

	/// <summary>
	/// Whether the decimal has been used in the text string yet
	/// </summary>
	bool PeriodUsed
	{
		get
		{
			return speed.Contains(".");
		}
	}

	/// <summary>
	/// The number of significant digits used in the text string
	/// </summary>
	int TextCharacters
	{
		get
		{
			return speed.Replace(".", "").Length;
		}
	}

	/// <summary>
	/// The text for the speed
	/// </summary>
	string speed = "2";



	/// <summary>
	/// When the game starts, init the components needed
	/// </summary>
	void Start()
	{
		InitComponents();
	}

	/// <summary>
	/// Every frame check for mouse click and key press
	/// </summary>
	void Update()
	{
		MouseClick();
		KeyPressed();
	}

	/// <summary>
	/// When the mouse is clicked update the speed button
	/// </summary>
	private void MouseClick()
	{
		if (Input.GetMouseButtonDown(0))
		{

			// The world position of the mouse
			Vector3 mousePosition =
				Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0;

			// If the button is clicked and the text is selected then enter the
			// current speed value
			if (!boxCollider.bounds.Contains(mousePosition))
			{
				if (textSelected)
				{
					text.color = black;
					textSelected = false;
					SpeedEntered();
				}
			}

			// If the button is not clicked, then deselect the speed button and
			// enter the current speed value
			else
			{

				text.color = textSelected ? black : blue;
				textSelected = !textSelected;
				SpeedEntered();
			}
		}
	}

	/// <summary>
	/// When a key is pressed, check to update the speed text
	/// </summary>
	private void KeyPressed()
	{
		if (textSelected)
		{

			// If the return key is pressed, then enter the current speed value
			if (Input.GetKeyDown(KeyCode.Return))
			{
				text.color = black;
				textSelected = false;
				SpeedEntered();
			}

			// If a number key is pressed, then enter that number
			for (int i = 0; i < 10; i++)
			{
				if (Input.GetKeyDown(i.ToString()))
				{
					KeyPressed(i.ToString());
					break;
				}
			}

			// If period key pressed, enter period if possible
			if (Input.GetKeyDown(KeyCode.Period))
			{
				KeyPressed(".");
			}

			// If backspace key pressed, then delete last character
			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				KeyPressed("Backspace");
			}
		}
	}

	/// <summary>
	/// Init the text mesh pro UGUI and box collider 2D components
	/// </summary>
	private void InitComponents()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	/// <summary>
	/// Sets the current speed value and display
	/// </summary>
	/// <param name="newSpeed">Speed value to set display</param>
	public void SetSpeed(float newSpeed)
	{

		// Set speed to two decimal places
		speed = newSpeed.ToString("0.00");

		// Allow only 3 significant digits
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

	/// <summary>
	/// Sets the speed value, display, and broadcasts speed to other button
	/// </summary>
	private void SpeedEntered()
	{

		// If last character is a period, then that character is deleted for
		// simplicity
		if (speed[speed.Length - 1] == '.')
		{
			speed = speed.Replace(".", "");
			text.text = speed + "x\nspeed";
		}

		// If the speed is 0, the speed defaults to 2x speed instead
		if (float.Parse(speed) == 0f)
		{
			speed = "2";
			text.text = speed + "x\nspeed";
		}

		// The speed value is broadcasted to the other speed button
		previewSpeed.SetSpeed(float.Parse(speed));
	}

	/// <summary>
	/// Edits the speed value display based on character input
	/// </summary>
	/// <param name="key">Key pressed ('0'-'9', '.', or 'Backspace')</param>
	private void KeyPressed(string key)
	{

		// If the period key is pressed, add a period if not already present,
		// and if there are not 3 characters for simplicity
		if (key.Equals(".") && !PeriodUsed && TextCharacters < 3)
		{
			speed += ".";
		}

		// If the backspace key is pressed, delete the last character, and if
		// there is only 1 character left, set speed display to 0
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

		// If a number character is pressed, 
		else if ("0123456789".Contains(key))
		{

			// If text is 0, replace speed with number, otherwise concat number
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

			// Shift all number characters 1 space over (skipping over period)
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

		// Update the text display
		text.text = speed + "x\nspeed";
	}

}