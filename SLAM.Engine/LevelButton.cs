using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Engine;

public class LevelButton : MonoBehaviour
{
	[SerializeField]
	protected UILabel lblLevelName;

	[SerializeField]
	protected UIButton btnLevelButton;

	private GameController.LevelSetting data;

	protected StartView view;

	public virtual void SetInfo(GameController.LevelSetting data, int index, StartView view, Game game)
	{
		this.data = data;
		this.view = view;
		lblLevelName.text = (index + 1).ToString();
	}

	public virtual void OnLevelSelected()
	{
		view.SelectLevel(data);
	}
}
