using UnityEngine;

namespace SLAM.TranslateThis;

public class TT_Key : MonoBehaviour
{
	private TT_GameController game;

	public char c { get; private set; }

	public void Init(TT_GameController game, char c)
	{
		this.game = game;
		this.c = c;
	}

	private void OnClick()
	{
		game.GuessLetter(c);
	}
}
