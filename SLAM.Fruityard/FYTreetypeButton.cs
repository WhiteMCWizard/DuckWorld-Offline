using System;
using UnityEngine;

namespace SLAM.Fruityard;

public class FYTreetypeButton : MonoBehaviour
{
	[Flags]
	private enum LevelSet
	{
		Level1 = 1,
		Level2 = 2,
		Level3 = 4,
		Level4 = 8,
		Level5 = 0x10,
		Level6 = 0x20,
		Level7 = 0x40,
		Level8 = 0x80,
		Level9 = 0x100,
		Level10 = 0x200,
		Level11 = 0x400,
		Level12 = 0x800,
		Level13 = 0x1000,
		Level14 = 0x2000,
		Level15 = 0x4000
	}

	public FruityardGame.FYTreeType TreeType;

	[SerializeField]
	[BitMask(typeof(LevelSet))]
	private LevelSet activeInLevels;

	[SerializeField]
	private AudioClip hoverSound;

	private void OnEnable()
	{
		GameEvents.Subscribe<FruityardGame.LevelInitEvent>(Init);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FruityardGame.LevelInitEvent>(Init);
	}

	private void Init(FruityardGame.LevelInitEvent evt)
	{
		base.gameObject.SetActive(((uint)activeInLevels & (uint)(1 << evt.Level)) == (uint)(1 << evt.Level));
	}

	private void OnHover(bool on)
	{
		if (on)
		{
			AudioController.Play(hoverSound.name);
		}
	}
}
