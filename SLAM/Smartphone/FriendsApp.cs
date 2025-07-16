using System;
using System.Collections.Generic;
using SLAM.SaveSystem;
using SLAM.Slinq;
using SLAM.Webservices;

namespace SLAM.Smartphone;

public class FriendsApp : AppController
{
	private UserProfile[] friends;

	protected override void Start()
	{
		base.Start();
		refreshNotifications();
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<InviteUserEvent>(onInviteUser);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<InviteUserEvent>(onInviteUser);
	}

	public override void Open()
	{
		OpenTempView<LoadingView>();
		DataStorage.GetFriends(delegate(UserProfile[] friends)
		{
			this.friends = friends;
			CloseTempView<LoadingView>();
			OpenView<FriendsView>().SetData(this.friends);
		});
	}

	public void Search(string name, Action<UserProfile[]> callback)
	{
	}

	private void onInviteUser(InviteUserEvent evt)
	{
	}

	protected override void checkForNotifications(Action<AppChangedEvent> eventCallback)
	{
	}

	internal void ShowProfile(UserProfile userProfile)
	{
		OpenView<ProfileView>().SetData(userProfile);
	}
}
