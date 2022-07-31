using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Converts frames from within the Assets folder to mp4 video format
/// </summary>
public class AnimationComputer
{

	/// <summary>
	/// 
	/// </summary>
	/// <param name="path">The path to the frames folder starting from within
	/// the Asset folder</param>
	/// <param name="animationName">Name of the animation frames</param>
	/// <param name="dimensions">How many pixels wide and tall each frame is
	/// (width, height)</param>
	/// <param name="framerate">Number of frames per second to make the
	/// animation</param>
	/// <param name="numFrames">The total number of frames in the folder</param>
	/// <param name="bitrate">The number of Mega bits per second (higher is
	/// better, 8 is good for 1080p video)</param>
	public static void ComputeAnimation(string path, string animationName,
		Vector2Int dimensions, int framerate, int numFrames, int bitrate)
	{

		// The process that will be executed on the command line
		Process process = new Process();
		process.StartInfo = new ProcessStartInfo();
		process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
		process.StartInfo.UseShellExecute = false;

		// Uses ffmpeg to create video
		process.StartInfo.FileName = Application.dataPath + "/ffmpeg";
		process.StartInfo.UseShellExecute = false;

		// Makes command line output redirection possible
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;

		// Image array to video conversion information
		string frameProcess = "-r " + framerate + " ";
		string framesPath = "-i " + Application.dataPath.Replace(" ", "' '") +
			"/" + path + "/";
		string frameNames = animationName + "%0" +
			Mathf.Ceil(Mathf.Log10(numFrames)) + "d.png ";
		string frameSize = "-s " + dimensions.x.ToString() + "x" +
			dimensions.y.ToString() + " ";
		string frameBitrate = "-b:v " + bitrate.ToString() + "M ";
		string framesPerSecond = "-framerate " + framerate.ToString() + " ";
		string animationOutput = Application.dataPath.Replace(" ", "' '") +
			"/" + path + "/" + animationName + "Animation.mp4";
		string fullArgument = frameProcess + framesPath + frameNames +
			frameSize + frameBitrate + framesPerSecond + animationOutput;

		// Executes command
		process.StartInfo.Arguments = fullArgument;
		process.Start();
		process.WaitForExit();
	}

}
