using System.IO;
using UnityEngine;

namespace CinemaDirector;

public class ScreenshotCapture : MonoBehaviour
{
	public string Folder = "CaptureOutput";

	public int FrameRate = 24;

	private void Start()
	{
		Time.captureFramerate = FrameRate;
		Directory.CreateDirectory(Folder);
	}

	private void Update()
	{
		string filename = $"{Folder}/shot {Time.frameCount:D04}.png";
		Application.CaptureScreenshot(filename);
	}
}
