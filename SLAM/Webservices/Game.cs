using System;
using LitJson;
using SLAM.Engine;
using SLAM.Slinq;

namespace SLAM.Webservices;

[Serializable]
public class Game
{
	public enum GameType
	{
		AdventureGame,
		Shop,
		Job,
		LocationGame,
		Event
	}

	public enum GameCharacter
	{
		NPC_AVATAR_NAME = -1,
		None,
		Major,
		NPC_NAME_GRANDMA_DUCK,
		NPC_NAME_GYRO_GEARLOOSE,
		NPC_NAME_SCROOGE,
		NPC_NAME_HUEY,
		NPC_NAME_MAY,
		NPC_NAME_CHRISQUIZ,
		NPC_NAME_WARBOL,
		NPC_NAME_GRANDMOGUL,
		NPC_NAME_DONALD_DUCK
	}

	[JsonName("free_to_play_levels")]
	public int[] FreeLevels;

	[JsonName("required_level_to_unlock")]
	public int RequiredDifficultyToUnlockNextGame;

	[JsonName("total_number_of_levels")]
	public int TotalLevels;

	[JsonName("location")]
	public int Location { get; set; }

	[JsonName("id")]
	public int Id { get; set; }

	[JsonName("name")]
	public string Name { get; set; }

	[JsonName("scene")]
	public string SceneName { get; set; }

	[JsonName("type")]
	public GameType Type { get; set; }

	[JsonName("enabled")]
	public bool Enabled { get; set; }

	[JsonName("is_unlocked")]
	public bool IsUnlocked { get; set; }

	[JsonName("is_unlocked_sa")]
	public bool IsUnlockedSA { get; set; }

	[JsonName("display_order")]
	public int SortOrder { get; set; }

	[JsonName("next_game")]
	public int? NextGameId { get; set; }

	[JsonName("previous_game")]
	public int? PreviousGameId { get; set; }

	[JsonName("scene_motioncomic")]
	public string SceneMotionComicName { get; set; }

	[JsonName("special_character")]
	public GameCharacter SpecialCharacter { get; set; }

	public int FreeLevelTo => FreeLevels.Max();

	public bool IsPremiumAvailable => true;

	public Game()
	{
	}

	public Game(int id, string name)
		: this()
	{
		Id = id;
		Name = name;
	}

	public Game(int id, string name, string sceneName)
		: this(id, name)
	{
		SceneName = sceneName;
	}

	public bool CanPlayLevel(GameController.LevelSetting level)
	{
		return FreeLevels.Contains(level.Index + 1);
	}

	public override string ToString()
	{
		return Name;
	}
}
