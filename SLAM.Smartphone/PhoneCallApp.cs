using System;
using System.Collections;
using SLAM.Avatar;
using SLAM.Invites;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone;

public class PhoneCallApp : AppController
{
	[SerializeField]
	private int ignoreCallAfterSeconds = 10;

	[SerializeField]
	private int hangupAfterResponseAfterSeconds = 5;

	private Animator phoneAnimator;

	private Game gameInvite;

	protected override void Start()
	{
		base.Start();
		phoneAnimator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<InviteSystem.GameInviteEvent>(onGameInviteReceived);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<InviteSystem.GameInviteEvent>(onGameInviteReceived);
	}

	public override void Open()
	{
		string calledId = Localization.Get(gameInvite.SpecialCharacter.ToString());
		Texture2D mugshotFor = SingletonMonobehaviour<PhotoBooth>.Instance.GetMugshotFor(gameInvite.SpecialCharacter);
		OpenView<ReceiveCallView>().SetData(calledId, mugshotFor);
		phoneAnimator.SetBool("BeingCalled", value: true);
		AudioController.Play("Interface_phone_ring");
		StartCoroutine("waitAndIgnoreCall");
	}

	public void AcceptCall()
	{
		AudioController.Stop("Interface_phone_ring");
		StopCoroutine("waitAndIgnoreCall");
		CloseView<ReceiveCallView>();
		phoneAnimator.SetTrigger("CallAccepted");
		phoneAnimator.SetBool("BeingCalled", value: false);
		phoneAnimator.SetBool("Visible", value: true);
		string sender = Localization.Get(gameInvite.SpecialCharacter.ToString());
		Texture2D mugshotFor = SingletonMonobehaviour<PhotoBooth>.Instance.GetMugshotFor(gameInvite.SpecialCharacter);
		string body = string.Empty;
		switch (gameInvite.Id)
		{
		case 12:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_ASSEMBLYLINE", UserProfile.Current.FirstName);
			break;
		case 26:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_CRATEMESS");
			break;
		case 36:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_DUCKQUIZ");
			break;
		case 10:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_FRUITYARD", UserProfile.Current.FirstName);
			break;
		case 24:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_HANGMAN", UserProfile.Current.FirstName);
			break;
		case 23:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_HIGHERTHAN", UserProfile.Current.FirstName);
			break;
		case 39:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_KARTRACINGTIMETRIAL", UserProfile.Current.FirstName);
			break;
		case 11:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_KARTRACING", UserProfile.Current.FirstName);
			break;
		case 2:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_MONEYDIVE");
			break;
		case 30:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_TRAINSPOTTING");
			break;
		case 33:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_TRANSLATETHIS", UserProfile.Current.FirstName);
			break;
		case 19:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_WARDROBE", UserProfile.Current.FirstName);
			break;
		case 22:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_FASHIONSTORE", UserProfile.Current.FirstName);
			break;
		case 29:
			body = StringFormatter.GetLocalizationFormatted("SF_INVITE_KARTSHOP", UserProfile.Current.FirstName);
			break;
		}
		OpenView<InboxItemView>().SetData(string.Empty, body, sender, DateTime.Now, mugshotFor, delegate
		{
			StartCoroutine(answerInvite(Localization.Get("SF_INVITE_ACCEPTED"), delegate
			{
				InviteSystem.AcceptGameInvitation();
				base.smartphone.Hide();
			}));
		}, delegate
		{
			StartCoroutine(answerInvite(Localization.Get("SF_INVITE_REJECTED"), delegate
			{
				InviteSystem.DeclineGameInvitation();
				base.smartphone.Hide();
			}));
		});
	}

	public void IgnoreCall()
	{
		InviteSystem.DeclineGameInvitation();
		AudioController.Stop("Interface_phone_ring");
		StopCoroutine("waitAndIgnoreCall");
		phoneAnimator.SetTrigger("CallRejected");
		phoneAnimator.SetBool("BeingCalled", value: false);
		base.smartphone.Hide();
	}

	private IEnumerator answerInvite(string body, Action actionAfter)
	{
		OpenView<InboxItemView>().SetData(body, null, null);
		yield return new WaitForSeconds(hangupAfterResponseAfterSeconds);
		actionAfter?.Invoke();
	}

	private void onGameInviteReceived(InviteSystem.GameInviteEvent evt)
	{
		InviteSystem.ReceivedGameInvitation();
		gameInvite = evt.Game;
		base.smartphone.ActivatePhoneCall();
	}

	protected override void checkForNotifications(Action<AppChangedEvent> eventCallback)
	{
	}

	private IEnumerator waitAndIgnoreCall()
	{
		yield return new WaitForSeconds(ignoreCallAfterSeconds);
		IgnoreCall();
	}
}
