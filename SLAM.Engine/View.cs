using UnityEngine;

namespace SLAM.Engine;

public class View : MonoBehaviour
{
	public delegate void Callback(View view);

	[SerializeField]
	private UITweener[] openCloseAnimations;

	private UITweener longestOpenAnimation;

	private UITweener longestCloseAnimation;

	private Callback openedCallback;

	private Callback closedCollback;

	private EventDelegate openedDelegate;

	private EventDelegate closedDelegate;

	private ViewController controller;

	private bool isOpen;

	public bool IsOpen => isOpen;

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	public virtual void Init(ViewController controller)
	{
		this.controller = controller;
		longestOpenAnimation = GetLongestAnimation(openCloseAnimations, includeDelay: true);
		longestCloseAnimation = GetLongestAnimation(openCloseAnimations, includeDelay: false);
	}

	public virtual void Open()
	{
		Open(null, immediately: false);
	}

	public virtual void Open(Callback callback)
	{
		Open(callback, immediately: false);
	}

	public virtual void Open(bool immediately)
	{
		Open(null, immediately);
	}

	public virtual void Open(Callback callback, bool immediately)
	{
		base.gameObject.SetActive(value: true);
		openedCallback = callback;
		isOpen = true;
		if (longestOpenAnimation != null)
		{
			openedDelegate = new EventDelegate(onOpenFinished);
			longestOpenAnimation.AddOnFinished(openedDelegate);
		}
		if (longestOpenAnimation == null || immediately)
		{
			onOpenFinished();
		}
		else
		{
			PlayAnimations(forward: true);
		}
	}

	public virtual void Close()
	{
		Close(null, immediately: false);
	}

	public virtual void Close(Callback callback)
	{
		Close(callback, immediately: false);
	}

	public virtual void Close(bool immediately)
	{
		Close(null, immediately);
	}

	public virtual void Close(Callback callback, bool immediately)
	{
		closedCollback = callback;
		if (longestCloseAnimation != null)
		{
			closedDelegate = new EventDelegate(onCloseFinished);
			longestCloseAnimation.AddOnFinished(closedDelegate);
		}
		if (longestCloseAnimation == null || immediately)
		{
			onCloseFinished();
		}
		else
		{
			PlayAnimations(forward: false);
		}
	}

	protected virtual void onOpenFinished()
	{
		if (longestOpenAnimation != null)
		{
			longestOpenAnimation.RemoveOnFinished(openedDelegate);
		}
		if (openedCallback != null)
		{
			openedCallback(this);
		}
	}

	protected virtual void onCloseFinished()
	{
		if (longestCloseAnimation != null)
		{
			longestCloseAnimation.RemoveOnFinished(closedDelegate);
		}
		isOpen = false;
		if (closedCollback != null)
		{
			closedCollback(this);
		}
		base.gameObject.SetActive(value: false);
	}

	private UITweener GetLongestAnimation(UITweener[] animations, bool includeDelay)
	{
		UITweener uITweener = null;
		if (animations.Length > 0)
		{
			uITweener = animations[0];
		}
		for (int i = 1; i < animations.Length; i++)
		{
			float num = animations[i].duration + ((!includeDelay) ? 0f : animations[i].delay);
			float num2 = uITweener.duration + ((!includeDelay) ? 0f : uITweener.delay);
			if (num > num2)
			{
				uITweener = animations[i];
			}
		}
		return uITweener;
	}

	private void PlayAnimations(bool forward)
	{
		for (int i = 0; i < openCloseAnimations.Length; i++)
		{
			if (forward)
			{
				openCloseAnimations[i].tweenFactor = 0f;
				openCloseAnimations[i].PlayForward();
			}
			else
			{
				openCloseAnimations[i].tweenFactor = 1f;
				openCloseAnimations[i].PlayReverse();
			}
		}
	}

	protected T Controller<T>() where T : ViewController
	{
		return controller as T;
	}
}
