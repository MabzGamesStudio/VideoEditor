using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Text for the skip button for the number of frames
/// </summary>
public class StepText : MonoBehaviour
{

	/// <summary>
	/// The text mesh pro UGUI display text
	/// </summary>
	private TextMeshProUGUI text;

	/// <summary>
	/// Box collider for the text
	/// </summary>
	private BoxCollider2D boxCollider;

	/// <summary>
	/// The preview skip button that corresponds to this text
	/// </summary>
	public PreviewSkip previewSkip;

	/// <summary>
	/// The selected color blue
	/// </summary>
	private Color blue = new Color(100f / 255f, 149f / 255f, 237f / 255f);

	/// <summary>
	/// The deselected color black
	/// </summary>
	private Color black = Color.black;

	/// <summary>
	/// Whether the text is selected
	/// </summary>
	private bool textSelected = false;

	/// <summary>
	/// The string version of the text to display
	/// </summary>
	private string frames = "10";

	/// <summary>
	/// Sets the text to the given frames number
	/// </summary>
	/// <param name="newFrames">New frames number</param>
	public void SetFrames(int newFrames)
	{
		frames = newFrames.ToString();
		text.text = frames + "x\nframes";
	}

	/// <summary>
	/// On the start of the game init the text components
	/// </summary>
	private void Start()
	{
		InitComponents();
	}

	/// <summary>
	/// On every frame check if the mouse is clicked or key has been pressed
	/// </summary>
	void Update()
	{
		OnMouseClicked();
		OnKeyPressed();
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
	/// If mouse is clicked, then apply proper select/deselect text action
	/// </summary>
	private void OnMouseClicked()
	{
		if (Input.GetMouseButtonDown(0))
		{


			// World mouse position
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0;

			// If the mouse clicked outside the text and the text is currently
			// selected, then deselect the text and update the frames
			if (!boxCollider.bounds.Contains(mousePosition))
			{
				if (textSelected)
				{
					text.color = black;
					textSelected = false;
					FramesEntered();
				}
			}

			// If the mouse clicked the text, then enter the frames and toggle
			// the select
			else
			{

				text.color = textSelected ? black : blue;
				textSelected = !textSelected;
				FramesEntered();
			}
		}
	}

	/// <summary>
	/// If the text is selected and a keyboard key is pressed, apply the proper
	/// text edit action
	/// </summary>
	private void OnKeyPressed()
	{
		if (textSelected)
		{

			// If retrun key pressed, enter frames and deselect text
			if (Input.GetKeyDown(KeyCode.Return))
			{
				text.color = black;
				textSelected = false;
				FramesEntered();
			}

			// If number key pressed, apply number pressed action
			for (int i = 0; i < 10; i++)
			{
				if (Input.GetKeyDown(i.ToString()))
				{
					KeyPressed(i.ToString());
					break;
				}
			}

			// If backspace key pressed, apply backspace action
			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				KeyPressed("Backspace");
			}
		}
	}

	/// <summary>
	/// Enters the frames number to the text display
	/// </summary>
	private void FramesEntered()
	{

		// 0 frames per skip is not allowed, so default value is set to 10
		if (int.Parse(frames) == 0)
		{
			frames = "10";
			text.text = frames + "x\nframes";
		}

		// Tells the skip button the updated frames amount
		previewSkip.SetFrames(int.Parse(frames));
	}

	/// <summary>
	/// Edits the text with the given backspace or number input
	/// </summary>
	/// <param name="key">String key pressed</param>
	private void KeyPressed(string key)
	{

		// Backspace key pressed
		if (key.Equals("Backspace"))
		{

			// If the frames is a single digit, set the value to 0
			if (frames.Length == 1)
			{
				frames = "0";
			}

			// Delete the last character
			else
			{
				frames = frames.Substring(0, frames.Length - 1);
			}
		}

		// Number key pressed
		else if ("0123456789".Contains(key))
		{

			// If the frames text length is less than 3
			if (frames.Length < 3)
			{

				// If the frames is 0, set the frames value to the entered key
				if (frames.Equals("0"))
				{
					frames = key;
				}

				// Otherwise append the digit to the back of the string
				else
				{
					frames += key;
				}
			}

			// If the frames text length is 3, append the new digit to the back,
			// then delete the first digit
			else
			{
				frames += key;
				frames = frames.Substring(1, frames.Length - 1);
			}
		}

		// Set the text display to the frames
		text.text = frames + "x\nframes";
	}

}
