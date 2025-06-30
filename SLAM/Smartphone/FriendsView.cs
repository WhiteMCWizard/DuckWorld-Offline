using SLAM.SaveSystem;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone;

public class FriendsView : AppView
{
	[SerializeField]
	private UIGrid friendsGrid;

	[SerializeField]
	private UIInput searchInputfield;

	[SerializeField]
	private GameObject friendRowPrefab;

	[SerializeField]
	private GameObject searchRowPrefab;

	private void OnDisable()
	{
		searchInputfield.onChange.Clear();
	}

	public void SetData(UserProfile[] friends)
	{
		searchInputfield.value = Localization.Get("SF_FRIENDS_DEFAULT_SEARCHFIELD_TEXT");
		searchInputfield.onChange.Add(new EventDelegate(this, "OnInputChanged"));
		friendsGrid.transform.DestroyChildren();
		friendsGrid.transform.DetachChildren();
		for (int i = 0; i < friends.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(friendsGrid.gameObject, friendRowPrefab);
			gameObject.GetComponent<FriendRow>().SetData(friends[i], Controller<FriendsApp>());
		}
		friendsGrid.enabled = true;
		friendsGrid.Reposition();
	}

	public void OnSubmitSearch()
	{
		if (searchInputfield.value.Length > 0)
		{
			Controller<FriendsApp>().Search(searchInputfield.value, onSearchResultsReceived);
		}
		else
		{
			onSearchResultsReceived(new UserProfile[0]);
		}
	}

	public void OnInputChanged()
	{
		searchInputfield.activeTextColor = new Color(0.3f, 0.3f, 0.3f);
	}

	private void onSearchResultsReceived(UserProfile[] results)
	{
		friendsGrid.transform.DestroyChildren();
		for (int i = 0; i < results.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(friendsGrid.gameObject, searchRowPrefab);
			gameObject.GetComponent<SearchResultRow>().SetData(results[i]);
			gameObject.name = results[i].FirstName + i;
		}
		UIGrid uIGrid = friendsGrid;
		bool repositionNow = true;
		friendsGrid.repositionNow = repositionNow;
		uIGrid.enabled = repositionNow;
	}
}
