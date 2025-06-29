using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Platformer;
using UnityEngine;

namespace SLAM.BatCave;

public class BatCaveGame : PlatformerGame
{
	private const int POINTS_PER_HEART = 100;

	private const int POINTS_PER_ANIMAL = 10;

	[SerializeField]
	private Material BackgroundMaterial;

	[SerializeField]
	private BatCaveDifficultySettings[] settings;

	private List<BC_ExoticAnimal.ExoticAnimalType> foundAnimals = new List<BC_ExoticAnimal.ExoticAnimalType>();

	private Texture originalBackgroundTexture;

	public override LevelSetting[] Levels => settings;

	public BatCaveDifficultySettings CurrentSettings => SelectedLevel<BatCaveDifficultySettings>();

	public int ExoticAnimalCount => foundAnimals.Count;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("BC_VICTORYWINDOW_SCORE_HEARTS_LEFT", hearts, 100), hearts * 100);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("BC_VICTORYWINDOW_SCORE_ANIMALS_COLLECTED", foundAnimals.Count, 10), foundAnimals.Count * 10);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_DONALD";

	public override string IntroTextKey => "BC_CINEMATICINTRO_TEXT";

	public override int GameId => 27;

	public override Portrait DuckCharacter => Portrait.DonaldDuck;

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		BatCaveDifficultySettings batCaveDifficultySettings = SelectedLevel<BatCaveDifficultySettings>();
		if (batCaveDifficultySettings.BackgroundTexture != null)
		{
			originalBackgroundTexture = BackgroundMaterial.mainTexture;
			BackgroundMaterial.mainTexture = batCaveDifficultySettings.BackgroundTexture;
		}
		for (int i = 0; i < settings.Length; i++)
		{
			settings[i].LevelRoot.SetActive(i == batCaveDifficultySettings.Index);
		}
		player.ResetPlayer(Vector3.zero);
		GetView<BC_HUDView>().SetInfo(foundAnimals.Count);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		BackgroundMaterial.mainTexture = originalBackgroundTexture;
	}

	protected override void onPickupCollectedEvent(PickupCollectedEvent evt)
	{
		if (evt.pickup is BC_ExoticAnimal)
		{
			foundAnimals.Add(((BC_ExoticAnimal)evt.pickup).Type);
			GetView<BC_HUDView>().SetInfo(foundAnimals.Count);
		}
	}

	protected override void onFinishReachedEvent(FinishReachedEvent evt)
	{
		Debug.LogError("Finish hasn't been implemented yet as we need an animation for that.");
		Finish(succes: true);
	}
}
