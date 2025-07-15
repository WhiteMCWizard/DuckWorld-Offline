using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using SLAM.Achievements;
using SLAM.Avatar;
using SLAM.Kart;
using SLAM.KartRacing;
using SLAM.SaveSystem;
using UnityEngine;

namespace SLAM.Webservices;

public static class ApiClient
{
	public static int UserId => UserProfile.Current.Id;

	public static string API_URL { get; private set; }

	private static string AUTHENTICATE_URL => API_URL + "/authenticate/";

	private static string LOGOUT_URL => API_URL + "/sanoma-account/logout/";

	private static string GET_FREE_TOKEN_URL => API_URL + "/freeplay-auth/";

	private static string USER_FIND_URL => API_URL + "/users/?find={0}";

	private static string USER_FRIENDS_URL => API_URL + "/users/{0}/friends/";

	private static string HIGHSCORES_URL => API_URL + "/users/{0}/games/{1}/highscores/?level={2}&difficulty={3}";

	private static string GHOST_URL => API_URL + "/users/{0}/ghosts/?game={1}&level={2}";

	private static string GHOST_URL2 => API_URL + "/users/{0}/ghosts/";

	private static string INVENTORY_BUY_URL => API_URL + "/users/{0}/inventory/buy/";

	private static string MESSAGES_DETAIL_URL => API_URL + "/users/{0}/messages/{1}/";

	private static string MESSAGES_ADDFRIEND_URL => API_URL + "/users/{0}/messages/addfriend/";

	private static string MESSAGES_CHALLENGEFRIEND_URL => API_URL + "/users/{0}/messages/challengefriend/";

	private static string MESSAGES_CHALLENGERESPONSE_URL => API_URL + "/users/{0}/messages/challengeresponse/";

	private static string TIP_A_PARENT_URL => API_URL + "/send-mail/";

	static ApiClient()
	{
		API_URL = "http://127.0.0.1:8000/api/1.0";
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ "--localhost", "http://127.0.0.1:8000/api/1.0" },
			{ "--test", "https://test.duckworld.com/api/1.0" },
			{ "--staging", "https://staging-new.duckworld.com/api/1.0" },
			{ "--production", "https://www.duckworld.com/api/1.0" }
		};
		IEnumerator enumerator = Environment.GetCommandLineArgs().GetEnumerator();
		while (enumerator.MoveNext())
		{
			string text = (string)enumerator.Current;
			if (dictionary.TryGetValue(text, out var value))
			{
				API_URL = value;
			}
			if (text == "--host" && enumerator.MoveNext())
			{
				API_URL = (string)enumerator.Current;
			}
		}
	}

	public static WebRequest GetFriends(Action<UserProfile[]> callback)
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_FRIENDS_URL, UserId), callback);
	}

	public static WebRequest Authenticate(string username, string passwrd, Action<bool> callback)
	{
		SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(null, string.Empty);
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(AUTHENTICATE_URL), new Dictionary<string, object>
		{
			{ "username", username },
			{ "password", passwrd }
		}, delegate(WebResponse response)
		{
			if (response.StatusCode == 200)
			{
				SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(JsonMapper.ToObject<AuthToken>(response.Request.text).Token, JsonMapper.ToObject<AuthToken>(response.Request.text).SessionID);
				callback(obj: true);
			}
			else
			{
				callback(obj: false);
			}
		});
	}

	public static WebRequest Logout()
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest<WebResponse>("POST", string.Format(LOGOUT_URL), new Dictionary<string, object> { 
		{
			"session_id",
			PlayerPrefs.GetString("session_id")
		} }, delegate
		{
		});
	}

	public static WebRequest GetFreeUserToken(Action<string> callback)
	{
		SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(null, string.Empty, persistent: false);
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", GET_FREE_TOKEN_URL, delegate(AuthToken token)
		{
			SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(token.Token, string.Empty, persistent: false);
			if (callback != null)
			{
				callback(token.Token);
			}
		});
	}

	public static WebRequest GetHighscores(int gameId, string difficulty, Action<HighScore[]> callback, string levelname = "default")
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(HIGHSCORES_URL, UserId.ToString(), gameId.ToString(), levelname, difficulty), callback);
	}

	public static WebRequest PurchaseItems(int[] shopItemIds, int shopId, Action<bool> callback = null)
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(INVENTORY_BUY_URL, UserId), new Dictionary<string, object> { { "shopitems", shopItemIds } }, validateResponseStatus(201, callback));
	}

	public static WebRequest GetTimeTrialConfiguration(int userId, int gameId, string difficulty, Action<GhostRecording[]> callback)
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(GHOST_URL, userId, gameId, difficulty), callback);
	}

	public static WebRequest LoadGhostRecording(GhostRecording recording, Action<GhostRecordingData> callback)
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", recording.RecordingUrl, delegate(WebResponse response)
		{
			if (callback != null)
			{
				GhostRecordingData obj = default(GhostRecordingData);
				if (response.StatusCode == 200)
				{
					try
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						MemoryStream serializationStream = new MemoryStream(response.Request.bytes);
						obj = (GhostRecordingData)binaryFormatter.Deserialize(serializationStream);
					}
					catch (Exception ex)
					{
						Debug.LogError("LoadGhostRecording failed!" + Environment.NewLine + ex);
						obj = default(GhostRecordingData);
					}
				}
				callback(obj);
			}
		});
	}

	public static WebRequest SubmitGhostRecording(int gameId, int difficulty, int elapsedMilliseconds, GhostRecordingData recording, Action<string> callback)
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("game", gameId);
		wWWForm.AddField("level", difficulty);
		wWWForm.AddField("gameuser", UserId.ToString());
		wWWForm.AddField("kart_config", JsonMapper.ToJson(recording.Kart));
		wWWForm.AddField("avatar_config", JsonMapper.ToJson(AvatarSystem.GetPlayerConfiguration()));
		wWWForm.AddField("time", elapsedMilliseconds);
		string hash = WebRequest.CalculateHash(wWWForm);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		MemoryStream memoryStream = new MemoryStream();
		binaryFormatter.Serialize(memoryStream, recording);
		wWWForm.AddBinaryData("coords", memoryStream.GetBuffer());
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(GHOST_URL2, UserId), hash, wWWForm, callback);
	}

	public static WebRequest SendFriendRequest(int recipientId, Action<bool> callback)
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(MESSAGES_ADDFRIEND_URL, UserId), new Dictionary<string, object> { 
		{
			"recipient",
			recipientId.ToString()
		} }, validateResponseStatus(201, callback));
	}

	public static WebRequest ChallengeFriend(int recipientId, int gameId, int score, string difficulty, Action<bool> callback)
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(MESSAGES_CHALLENGEFRIEND_URL, UserId), new Dictionary<string, object>
		{
			{
				"recipient",
				recipientId.ToString()
			},
			{
				"game",
				gameId.ToString()
			},
			{
				"score_sender",
				score.ToString()
			},
			{ "difficulty", difficulty }
		}, validateResponseStatus(201, callback));
	}

	public static WebRequest SendChallengeResult(int recipientId, int game, string difficulty, int score_sender, int score_recipient, Action<bool> callback)
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", string.Format(MESSAGES_CHALLENGERESPONSE_URL, UserId), new Dictionary<string, object>
		{
			{
				"recipient",
				recipientId.ToString()
			},
			{
				"game",
				game.ToString()
			},
			{
				"difficulty",
				difficulty.ToString()
			},
			{
				"score_sender",
				score_sender.ToString()
			},
			{
				"score_recipient",
				score_recipient.ToString()
			}
		}, validateResponseStatus(201, callback));
	}

	public static WebRequest UpdateScoreRecipientForChallenge(int messageId, int score_recipient, Action<bool> callback)
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("PATCH", string.Format(MESSAGES_DETAIL_URL, UserId, messageId), new Dictionary<string, object> { 
		{
			"score_recipient",
			score_recipient.ToString()
		} }, validateResponseStatus(201, callback));
	}

	public static WebRequest SearchPlayerByName(string name, Action<UserProfile[]> callback)
	{
		name = Uri.EscapeDataString(name);
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("GET", string.Format(USER_FIND_URL, name), callback);
	}

	public static void OpenRegisterPage()
	{
	}

	public static void OpenPropositionPage(int gameId)
	{
	}

	public static void OpenForgotPasswordPage()
	{
	}

	public static void OpenHelpPage()
	{
		Application.OpenURL("https://github.com/WhiteMCWizard/DuckWorld-Offline/issues");
	}

	public static WebRequest TipAParent(string recipient, Action<bool> callback)
	{
		return SingletonMonobehaviour<Webservice>.Instance.DoRequest("POST", TIP_A_PARENT_URL, new Dictionary<string, object>
		{
			{ "toname", recipient },
			{ "subject", "Tip DuckWorld.com" }
		}, validateResponseStatus(200, callback));
	}

	private static Action<WebResponse> validateResponseStatus(int requiredStatus, Action<bool> callback)
	{
		return delegate(WebResponse response)
		{
			if (callback != null)
			{
				callback(response.StatusCode == requiredStatus);
			}
		};
	}
}
