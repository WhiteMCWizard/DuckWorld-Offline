using UnityEngine;

public class AudioFader
{
	private float _fadeOutTotalTime = -1f;

	private double _fadeOutStartTime = -1.0;

	private float _fadeInTotalTime = -1f;

	private double _fadeInStartTime = -1.0;

	public double time { get; set; }

	public bool isFadingOutComplete
	{
		get
		{
			if (_fadeOutStartTime > 0.0)
			{
				return _fadeOutTotalTime >= 0f && time >= _fadeOutStartTime + (double)_fadeOutTotalTime;
			}
			return _fadeOutTotalTime >= 0f && time >= (double)_fadeOutTotalTime;
		}
	}

	public bool isFadingOut
	{
		get
		{
			if (_fadeOutStartTime > 0.0)
			{
				return _fadeOutTotalTime >= 0f && time >= _fadeOutStartTime && time < _fadeOutStartTime + (double)_fadeOutTotalTime;
			}
			return _fadeOutTotalTime >= 0f && time < (double)_fadeOutTotalTime;
		}
	}

	public bool isFadingOutOrScheduled => _fadeOutTotalTime >= 0f;

	public bool isFadingIn
	{
		get
		{
			if (_fadeInStartTime > 0.0)
			{
				return _fadeInTotalTime > 0f && time >= _fadeInStartTime && time - _fadeInStartTime < (double)_fadeInTotalTime;
			}
			return _fadeInTotalTime > 0f && time < (double)_fadeInTotalTime;
		}
	}

	public void Set0()
	{
		time = 0.0;
		_fadeOutTotalTime = -1f;
		_fadeOutStartTime = -1.0;
		_fadeInTotalTime = -1f;
		_fadeInStartTime = -1.0;
	}

	public void FadeIn(float fadeInTime, bool stopCurrentFadeOut = false)
	{
		FadeIn(fadeInTime, time, stopCurrentFadeOut);
	}

	public void FadeIn(float fadeInTime, double startToFadeTime, bool stopCurrentFadeOut = false)
	{
		if (isFadingOutOrScheduled && stopCurrentFadeOut)
		{
			float num = _GetFadeOutValue();
			_fadeOutTotalTime = -1f;
			_fadeOutStartTime = -1.0;
			_fadeInTotalTime = fadeInTime;
			_fadeInStartTime = startToFadeTime - (double)(fadeInTime * num);
		}
		else
		{
			_fadeInTotalTime = fadeInTime;
			_fadeInStartTime = startToFadeTime;
		}
	}

	public void FadeOut(float fadeOutLength, float startToFadeTime)
	{
		if (isFadingOutOrScheduled)
		{
			double num = time + (double)startToFadeTime + (double)fadeOutLength;
			double num2 = _fadeOutStartTime + (double)_fadeOutTotalTime;
			if (!(num2 < num))
			{
				double num3 = time - _fadeOutStartTime;
				double num4 = startToFadeTime + fadeOutLength;
				double num5 = num2 - time;
				if (num5 != 0.0)
				{
					double num6 = num3 * num4 / num5;
					_fadeOutStartTime = time - num6;
					_fadeOutTotalTime = (float)(num4 + num6);
				}
			}
		}
		else
		{
			_fadeOutTotalTime = fadeOutLength;
			_fadeOutStartTime = time + (double)startToFadeTime;
		}
	}

	public float Get(out bool finishedFadeOut)
	{
		float num = 1f;
		finishedFadeOut = false;
		if (isFadingOutOrScheduled)
		{
			num *= _GetFadeOutValue();
			if (num == 0f)
			{
				finishedFadeOut = true;
				return 0f;
			}
		}
		if (isFadingIn)
		{
			num *= _GetFadeInValue();
		}
		return num;
	}

	private float _GetFadeOutValue()
	{
		return 1f - _GetFadeValue((float)(time - _fadeOutStartTime), _fadeOutTotalTime);
	}

	private float _GetFadeInValue()
	{
		return _GetFadeValue((float)(time - _fadeInStartTime), _fadeInTotalTime);
	}

	private float _GetFadeValue(float t, float dt)
	{
		if (dt <= 0f)
		{
			return (!(t > 0f)) ? 0f : 1f;
		}
		return Mathf.Clamp(t / dt, 0f, 1f);
	}
}
