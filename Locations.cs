using System;
using SLAM.Engine;
using SLAM.Webservices;

public static class Locations
{
    public static Location[] GetLocations()
    {
        return new Location[]
        {
            // HUB_LOCATION_WOODCHUCKS
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 4,
                        Id = 29,
                        Name = "HUB_GAME_KARTSHOP",
                        SceneName = "Kartshop",
                        Type = Game.GameType.Shop,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        NextGameId = 2,
                        PreviousGameId = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_GRANDMOGUL,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 0
                    },
                    new Game {
                        Location = 4,
                        Id = 11,
                        Name = "HUB_GAME_KARTRACER",
                        SceneName = "KartRacing",
                        Type = Game.GameType.LocationGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 2,
                        NextGameId = 3,
                        PreviousGameId = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_HUEY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 10
                    },
                    new Game {
                        Location = 4,
                        Id = 39,
                        Name = "HUB_GAME_KARTRACERTIMETRIAL",
                        SceneName = "KartRacingTimeTrial",
                        Type = Game.GameType.LocationGame,
                        Enabled = false,
                        IsUnlocked = false,
                        IsUnlockedSA = false,
                        SortOrder = 3,
                        PreviousGameId = 2,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_HUEY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 6
                    }
                },
                Id = 4,
                Name = "HUB_LOCATION_WOODCHUCKS",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_MONEY_BIN
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 1,
                        Id = 2,
                        Name = "HUB_GAME_MONEYDIVE",
                        SceneName = "MoneyDive",
                        Type = Game.GameType.LocationGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_SCROOGE,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    }
                },
                Id = 1,
                Name = "HUB_LOCATION_MONEY_BIN",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_HARBOR
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 7,
                        Id = 34,
                        Name = "HUB_GAME_MOTIONCOMIC_ADVENTURE1_INTRO",
                        SceneName = "Hub",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        NextGameId = 5,
                        SceneMotionComicName = "MC_ADV01_01_Intro",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 1
                    },
                    new Game {
                        Location = 7,
                        Id = 5,
                        Name = "HUB_GAME_CRANEOPERATOR",
                        SceneName = "CraneOperator",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 2,
                        NextGameId = 6,
                        PreviousGameId = 34,
                        SceneMotionComicName = "MC_ADV01_02_CO",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    },
                    new Game {
                        Location = 7,
                        Id = 6,
                        Name = "HUB_GAME_CHASETHEBOAT",
                        SceneName = "ChaseTheBoat",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 3,
                        NextGameId = 7,
                        PreviousGameId = 5,
                        SceneMotionComicName = "MC_ADV01_03_CTB",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 5
                    },
                    new Game {
                        Location = 7,
                        Id = 7,
                        Name = "HUB_GAME_CHASETHETRUCK",
                        SceneName = "FollowTheTruck",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 4,
                        NextGameId = 8,
                        PreviousGameId = 6,
                        SceneMotionComicName = "MC_ADV01_04_CTT",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 5
                    },
                    new Game {
                        Location = 7,
                        Id = 8,
                        Name = "HUB_GAME_PUSHTHECRATE",
                        SceneName = "PushTheCrate",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 5,
                        NextGameId = 9,
                        PreviousGameId = 7,
                        SceneMotionComicName = "MC_ADV01_05_PTC",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 5
                    },
                    new Game {
                        Location = 7,
                        Id = 9,
                        Name = "HUB_GAME_ZOOTRANSPORT",
                        SceneName = "ZooTransport",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 6,
                        NextGameId = 35,
                        PreviousGameId = 8,
                        SceneMotionComicName = "MC_ADV01_06_ZT",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    },
                    new Game {
                        Location = 7,
                        Id = 35,
                        Name = "HUB_GAME_MOTIONCOMIC_ADVENTURE1_OUTRO",
                        SceneName = "Hub",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 7,
                        NextGameId = 37,
                        PreviousGameId = 9,
                        SceneMotionComicName = "MC_ADV01_07_Outro",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 1
                    }
                },
                Id = 7,
                Name = "HUB_LOCATION_HARBOR",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_ZOO
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 11,
                        Id = 37,
                        Name = "HUB_GAME_MOTIONCOMIC_ADVENTURE2_INTRO",
                        SceneName = "Hub",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        NextGameId = 4,
                        SceneMotionComicName = "MC_ADV02_01_Intro",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 1
                    },
                    new Game {
                        Location = 11,
                        Id = 4,
                        Name = "HUB_GAME_CONNECTTHEPIPES",
                        SceneName = "ConnectThePipes",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 2,
                        NextGameId = 16,
                        PreviousGameId = 37,
                        SceneMotionComicName = "MC_ADV02_02_CTP",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    },
                    new Game {
                        Location = 11,
                        Id = 16,
                        Name = "HUB_GAME_BEATTHEBEAGLEBOYS",
                        SceneName = "BeatTheBeagleBoys",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 3,
                        NextGameId = 27,
                        PreviousGameId = 4,
                        SceneMotionComicName = "MC_ADV02_03_BTB",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 5
                    },
                    new Game {
                        Location = 11,
                        Id = 27,
                        Name = "HUB_GAME_BATCAVE",
                        SceneName = "BatCave",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 4,
                        NextGameId = 1,
                        PreviousGameId = 16,
                        SceneMotionComicName = "MC_ADV02_04_BC",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 5
                    },
                    new Game {
                        Location = 11,
                        Id = 1,
                        Name = "HUB_GAME_JUMPTHECROC",
                        SceneName = "JumpTheCroc",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 5,
                        NextGameId = 28,
                        PreviousGameId = 27,
                        SceneMotionComicName = "MC_ADV02_05_JTC",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    },
                    new Game {
                        Location = 11,
                        Id = 28,
                        Name = "HUB_GAME_MONKEYBATTLE",
                        SceneName = "MonkeyBattle",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 6,
                        NextGameId = 38,
                        PreviousGameId = 1,
                        SceneMotionComicName = "MC_ADV02_06_MB",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 5
                    },
                    new Game {
                        Location = 11,
                        Id = 38,
                        Name = "HUB_GAME_MOTIONCOMIC_ADVENTURE2_OUTRO",
                        SceneName = "Hub",
                        Type = Game.GameType.AdventureGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 7,
                        PreviousGameId = 28,
                        SceneMotionComicName = "MC_ADV02_07_Outro",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 1
                    }
                },
                Id = 11,
                Name = "HUB_LOCATION_ZOO",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_GRANDMAS_FARM
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 2,
                        Id = 10,
                        Name = "HUB_GAME_FRUITYARD",
                        SceneName = "Fruityard",
                        Type = Game.GameType.Job,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_GRANDMA_DUCK,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    }
                },
                Id = 2,
                Name = "HUB_LOCATION_GRANDMAS_FARM",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_GYROS_WORKSHOP
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 3,
                        Id = 12,
                        Name = "HUB_GAME_ASSEMBLYLINE",
                        SceneName = "AssemblyLine",
                        Type = Game.GameType.Job,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_GYRO_GEARLOOSE,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    }
                },
                Id = 3,
                Name = "HUB_LOCATION_GYROS_WORKSHOP",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_SCHOOL
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 6,
                        Id = 23,
                        Name = "HUB_GAME_HIGHERTHAN",
                        SceneName = "HigherThan",
                        Type = Game.GameType.LocationGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        NextGameId = 2,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_HUEY,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    },
                    new Game {
                        Location = 6,
                        Id = 24,
                        Name = "HUB_GAME_HANGMAN",
                        SceneName = "Hangman",
                        Type = Game.GameType.LocationGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 2,
                        NextGameId = 3,
                        PreviousGameId = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_WARBOL,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    },
                    new Game {
                        Location = 6,
                        Id = 33,
                        Name = "HUB_GAME_TRANSLATETHIS",
                        SceneName = "TranslateThis",
                        Type = Game.GameType.LocationGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 2,
                        NextGameId = 7,
                        PreviousGameId = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_WARBOL,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    }
                },
                Id = 6,
                Name = "HUB_LOCATION_SCHOOL",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_STATION
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 14,
                        Id = 30,
                        Name = "HUB_GAME_TRAINSPOTTING",
                        SceneName = "TrainSpotting",
                        Type = Game.GameType.Job,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        NextGameId = 2,
                        PreviousGameId = 5,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_SCROOGE,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    },
                    new Game {
                        Location = 14,
                        Id = 26,
                        Name = "HUB_GAME_CRATEMESS",
                        SceneName = "Cratemess",
                        Type = Game.GameType.Job,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 2,
                        PreviousGameId = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_SCROOGE,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    }
                },
                Id = 14,
                Name = "HUB_LOCATION_STATION",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_TVSTUDIO
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 36,
                        Id = 36,
                        Name = "HUB_GAME_DUCKQUIZ",
                        SceneName = "DuckQuiz",
                        Type = Game.GameType.LocationGame,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        NextGameId = 2,
                        PreviousGameId = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_CHRISQUIZ,
                        FreeLevels = new int[] { 1 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 15
                    }
                },
                Id = 15,
                Name = "HUB_LOCATION_TVSTUDIO",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_SHOPPING_STREET
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 12,
                        Id = 22,
                        Name = "HUB_GAME_FASHIONSTORE",
                        SceneName = "FashionStore",
                        Type = Game.GameType.Shop,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_NAME_MAY,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 0
                    }
                },
                Id = 12,
                Name = "HUB_LOCATION_SHOPPING_STREET",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_AVATAR_HOUSE
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 5,
                        Id = 19,
                        Name = "HUB_GAME_WARDROBE",
                        SceneName = "Wardrobe",
                        Type = Game.GameType.Shop,
                        Enabled = true,
                        IsUnlocked = true,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_AVATAR_NAME,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 0
                    }
                },
                Id = 5,
                Name = "HUB_LOCATION_AVATAR_HOUSE",
                Description = "",
                Enabled = true,
                IsUnlocked = true
            },
            // HUB_LOCATION_MEETINGPOINT
            new Location {
                Games = new Game[] {
                    new Game {
                        Location = 16,
                        Id = 40,
                        Name = "HUB_GAME_MEETINGPOINT",
                        SceneName = "MeetingPoint",
                        Type = Game.GameType.LocationGame,
                        Enabled = false,
                        IsUnlocked = false,
                        IsUnlockedSA = false,
                        SortOrder = 1,
                        SceneMotionComicName = "",
                        SpecialCharacter = Game.GameCharacter.NPC_AVATAR_NAME,
                        FreeLevels = new int[] { 0 },
                        RequiredDifficultyToUnlockNextGame = 1,
                        TotalLevels = 0
                    }
                },
                Id = 16,
                Name = "HUB_LOCATION_MEETINGPOINT",
                Description = "",
                Enabled = false,
                IsUnlocked = false
            }
        };
    }

    public static int GetRequiredDifficultyToUnlockNextGame(int gameId)
    {
        foreach (var location in GetLocations())
        {
            foreach (var game in location.Games)
            {
                if (game.Id == gameId)
                    return game.RequiredDifficultyToUnlockNextGame;
            }
        }
        return -1;
    }
}
