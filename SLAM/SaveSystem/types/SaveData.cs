using System;
using SLAM.Achievements;
using SLAM.Kart;
using SLAM.Webservices;

namespace SLAM.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public UserProfile profile = new UserProfile();
        public PlayerAvatarData avatar = new PlayerAvatarData();
        public KartConfigurationData[] kartConfigurations = new KartConfigurationData[0];
        public UserGameDetails[] userGameDetails = new UserGameDetails[0];
        public UserAchievement[] userAchievements = new UserAchievement[0];
        public Message[] messages = new Message[0];
        public PurchasedShopItemData[] purchasedShopItems = new PurchasedShopItemData[0];
        public int walletTotal = 0;
        public int saveVersion = 0;

        [NonSerialized]
        public const int CURRENT_VERSION = 1;

        // Add more fields here as needed for future data

        public void MigrateAndValidate()
        {
            // Ensure arrays are never null (old saves may have null)
            if (profile == null) profile = new UserProfile();
            if (avatar == null) avatar = new PlayerAvatarData();
            if (kartConfigurations == null) kartConfigurations = new KartConfigurationData[0];
            if (userGameDetails == null) userGameDetails = new UserGameDetails[0];
            if (userAchievements == null) userAchievements = new UserAchievement[0];
            if (messages == null) messages = new Message[0];
            if (purchasedShopItems == null) purchasedShopItems = new PurchasedShopItemData[0];
            if (walletTotal < 0) walletTotal = 0;

            // Apply migrations based on version
            if (saveVersion < CURRENT_VERSION)
            {
                // Future migrations go here
                saveVersion = CURRENT_VERSION;
            }
        }

        public void SaveScore(int gameId, int score, string difficulty, int elapsedMilliseconds, bool gameCompleted, Action<UserScore> callback)
        {
            SaveScore(gameId, score, difficulty, "default", elapsedMilliseconds, gameCompleted, callback);
        }

        public void SaveScore(int gameId, int score, string difficulty, string levelName, int elapsedMilliseconds, bool gameCompleted, Action<UserScore> callback)
        {
            // Find or create UserGameDetails for the given gameId
            var gameDetails = Array.Find(userGameDetails, g => g.GameId == gameId);
            if (gameDetails == null)
            {
                gameDetails = new UserGameDetails
                {
                    Id = gameId,
                    IsUnlocked = true,
                    IsUnlockedSA = false,
                    HasFinished = gameCompleted,
                    Progression = new UserGameProgression[0],
                    GameId = gameId
                };
                Array.Resize(ref userGameDetails, userGameDetails.Length + 1);
                userGameDetails[userGameDetails.Length - 1] = gameDetails;
            }

            if (gameCompleted)
            {
                gameDetails.HasFinished = true;
            }

            // Find or create progression for the given level
            int diffIndex;
            int.TryParse(difficulty, out diffIndex);
            var progression = Array.Find(gameDetails.Progression, p => p.LevelIndex == diffIndex);
            if (progression == null)
            {
                var newProgression = new UserGameProgression
                {
                    LevelIndex = diffIndex,
                    Level = levelName,
                    Score = score,
                    Time = elapsedMilliseconds
                };
                Array.Resize(ref gameDetails.Progression, gameDetails.Progression.Length + 1);
                gameDetails.Progression[gameDetails.Progression.Length - 1] = newProgression;
            }
            else
            {
                if (progression.Score < score)
                    progression.Score = score;
                if (progression.Time > elapsedMilliseconds || progression.Time == 0)
                    progression.Time = elapsedMilliseconds;
            }

            var requiredLevel = Locations.GetRequiredDifficultyToUnlockNextGame(gameId);

            if (int.TryParse(difficulty, out int difficultyValue) && difficultyValue >= requiredLevel)
            {
                // Unlock next level if applicable
                int? nextGameId = Locations.GetNextGameId(gameId);
                if (nextGameId.HasValue)
                {
                    var nextGameDetails = Array.Find(userGameDetails, g => g.GameId == nextGameId.Value);
                    if (nextGameDetails == null)
                    {
                        nextGameDetails = new UserGameDetails
                        {
                            Id = nextGameId.Value,
                            IsUnlocked = true,
                            IsUnlockedSA = false,
                            HasFinished = false,
                            Progression = new UserGameProgression[0],
                            GameId = nextGameId.Value
                        };
                        Array.Resize(ref userGameDetails, userGameDetails.Length + 1);
                        userGameDetails[userGameDetails.Length - 1] = nextGameDetails;
                    }
                }
            }

            SaveManager.Instance.MarkDirty();
            callback?.Invoke(new UserScore());
        }

        public void SaveKartConfiguration(KartConfigurationData kartConfig, byte[] image, Action<KartConfigurationData> callback)
        {
            if (kartConfigurations == null || kartConfigurations.Length == 0)
            {
                kartConfigurations = new KartConfigurationData[] { kartConfig };
            }
            else
            {
                kartConfigurations[0] = kartConfig;
            }

            SaveManager.Instance.MarkDirty();

            callback?.Invoke(kartConfig);
        }
    }
}