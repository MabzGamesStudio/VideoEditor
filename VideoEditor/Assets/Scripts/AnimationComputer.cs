using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class AnimationComputer
{

	/// <summary>
	/// 
	/// </summary>
	/// <param name="path">The path to the frames folder starting from within the Asset folder</param>
	/// <param name="animationName">Name of the animation frames</param>
	/// <param name="dimensions">How many pixels wide and tall each frame is (width, height)</param>
	/// <param name="framerate">Number of frames per second to make the animation</param>
	/// <param name="numFrames">The total number of frames in the folder</param>
	public static void ComputeAnimation(string path, string animationName, Vector2Int dimensions, int framerate, int numFrames)
	{

		Process process = new Process();
		process.StartInfo = new ProcessStartInfo();
		process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.FileName = Application.dataPath + "/ffmpeg";

		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;

		string frameProcess = "-r 1 ";
		string framesPath = "-i " + Application.dataPath.Replace(" ", "' '") + "/" + path + "/";
		string frameNames = animationName + "%0" + Mathf.Ceil(numFrames / 10f) + "d.png ";
		string frameSize = "-s " + dimensions.x.ToString() + "x" + dimensions.y.ToString() + " ";
		string framesPerSecond = "-framerate " + framerate.ToString() + " ";
		string animationOutput = Application.dataPath.Replace(" ", "' '") + "/" + path + "/" + animationName + "Animation.mp4";

		string fullArgument = frameProcess + framesPath + frameNames + frameSize + framesPerSecond + animationOutput;

		process.StartInfo.Arguments = fullArgument;

		process.Start();
		process.WaitForExit();
	}

}
