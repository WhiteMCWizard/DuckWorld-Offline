using UnityEngine;

namespace SLAM.Shared;

public class UIAudio : MonoBehaviour
{
	public enum InterfaceSounds
	{
		Interface_buttonClick_primary,
		Interface_buttonClick_secundary,
		Interface_buttonClick_alt,
		Interface_buttonClick_switch,
		Interface_buttonClick_change,
		Interface_buttonClick_arrow,
		Interface_window_close,
		Interface_window_open,
		Interface_buttonClick_phone
	}

	[SerializeField]
	private InterfaceSounds onClickSound;

	private bool wasOn;

	private void OnClick()
	{
		playSound(onClickSound.ToString());
	}

	private void OnHover(bool hoverOn)
	{
		if (!wasOn && hoverOn && onClickSound == InterfaceSounds.Interface_buttonClick_primary)
		{
			playSound("Interface_button_mouseOver");
		}
		wasOn = hoverOn;
	}

	private void playSound(string id)
	{
		if (!string.IsNullOrEmpty(id) && (bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			AudioController.Play(id);
		}
	}
}
