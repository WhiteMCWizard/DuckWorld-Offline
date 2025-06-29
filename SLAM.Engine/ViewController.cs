using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Engine;

public class ViewController : MonoBehaviour
{
	private List<View> allViews = new List<View>();

	protected virtual void Start()
	{
	}

	protected void AddView(View view)
	{
		if (view == null)
		{
			Debug.LogError("Cannot add view as it is null.");
		}
		else if (!allViews.Contains(view))
		{
			allViews.Add(view);
			view.Init(this);
			view.Close(immediately: true);
		}
		else
		{
			Debug.LogError("Cannot add view: \"" + view.GetType().Name + "\" as it has already been added.", view);
		}
	}

	protected void AddViews(params View[] newViews)
	{
		for (int i = 0; i < newViews.Length; i++)
		{
			AddView(newViews[i]);
		}
	}

	protected void OpenView(View view)
	{
		OpenView(view, null);
	}

	protected virtual void OpenView(View view, View.Callback callback)
	{
		if (allViews.Contains(view))
		{
			if (!view.IsOpen)
			{
				view.Open(callback);
			}
			else
			{
				Debug.LogError("View " + view.GetType().Name + " is already open.", view);
			}
		}
		else
		{
			Debug.LogError("Can't open view since its unknown to me." + view, this);
		}
	}

	protected T OpenView<T>(View.Callback callback) where T : View
	{
		View view = GetView<T>();
		OpenView(view, callback);
		return view as T;
	}

	protected T OpenView<T>() where T : View
	{
		return OpenView<T>(null);
	}

	protected void CloseView(View view)
	{
		CloseView(view, null);
	}

	protected virtual void CloseView(View view, View.Callback callback)
	{
		if (allViews.Contains(view))
		{
			if (view.IsOpen)
			{
				view.Close(callback);
			}
			else
			{
				Debug.LogError("View " + view.GetType().Name + " is already closed.", view);
			}
		}
		else
		{
			Debug.LogError("Can't close view since its unknown to me." + view, this);
		}
	}

	protected void CloseView<T>(View.Callback callback) where T : View
	{
		View view = GetView<T>();
		if (!(view == null))
		{
			if (!view.IsOpen)
			{
				Debug.LogError("Cannot close view since it's not open. " + view.GetType().Name, this);
			}
			else
			{
				CloseView(view, callback);
			}
		}
	}

	protected void CloseView<T>() where T : View
	{
		CloseView<T>(null);
	}

	protected void CloseAllViews()
	{
		foreach (View allView in allViews)
		{
			if (allView.IsOpen)
			{
				CloseView(allView);
			}
		}
	}

	protected void CloseAllViews(params Type[] excluded)
	{
		foreach (View allView in allViews)
		{
			if (!excluded.Contains(allView.GetType()) && allView.IsOpen)
			{
				CloseView(allView);
			}
		}
	}

	protected bool IsViewOpen<T>() where T : View
	{
		View view = GetView<T>();
		if (view != null)
		{
			return view.IsOpen;
		}
		return false;
	}

	protected T GetView<T>() where T : View
	{
		for (int i = 0; i < allViews.Count; i++)
		{
			if (allViews[i] is T)
			{
				return allViews[i] as T;
			}
		}
		Debug.LogError("Can't find view: " + typeof(T).Name, this);
		return (T)null;
	}

	protected bool HasView<T>() where T : View
	{
		return allViews.Any((View view) => view is T);
	}

	protected virtual IEnumerator OpenAndWait<T>() where T : View
	{
		bool isDoneOpening = false;
		bool flag;
		OpenView<T>(delegate
		{
			flag = true;
		});
		while (!isDoneOpening)
		{
			yield return null;
		}
	}

	protected virtual IEnumerator CloseAndWait<T>() where T : View
	{
		bool isDoneClosing = false;
		bool flag;
		CloseView<T>(delegate
		{
			flag = true;
		});
		while (!isDoneClosing)
		{
			yield return null;
		}
	}
}
