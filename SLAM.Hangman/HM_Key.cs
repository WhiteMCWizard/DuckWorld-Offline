using UnityEngine;

namespace SLAM.Hangman;

public class HM_Key : MonoBehaviour
{
	private HangmanGame game;

	private char c;

	public void Init(HangmanGame game, char c)
	{
		this.game = game;
		this.c = c;
	}

	private void OnClick()
	{
		game.GuessLetter(c);
	}
}
