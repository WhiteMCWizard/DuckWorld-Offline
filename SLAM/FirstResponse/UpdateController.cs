using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.FirstResponse;

public class UpdateController : MonoBehaviour
{
	[SerializeField]
	private UILabel statusLabel;

	[SerializeField]
	private UIProgressBar progressbar;

	private void Start()
	{
	}

	private IEnumerator doUpdateSequence(WebConfiguration config)
	{
		yield return null;
	}

	private void setStatus(string status)
	{
		statusLabel.text = status;
	}
}
