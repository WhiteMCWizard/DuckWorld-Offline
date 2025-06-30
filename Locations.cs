using LitJson;
using SLAM.Webservices;
using System.Collections.Generic;

public class Locations
{
    public static Location[] All = new Location[]
    {
        new Location(4, "HUB_LOCATION_WOODCHUCKS", new Game[]
		{
			new Game(29, "HUB_GAME_KARTSHOP"),
			new Game(11, "HUB_GAME_KARTRACER"),
			new Game(39, "HUB_GAME_KARTRACERTIMETRIAL")
		}),
        new Location(1, "HUB_LOCATION_MONEY_BIN", new Game[]
        {
            new Game(2, "HUB_GAME_MONEYDIVE")
        }),
        new Location(7, "HUB_LOCATION_HARBOR", new Game[]
        {
            new Game(34, "HUB_GAME_MOTIONCOMIC_ADVENTURE1_INTRO"),
            new Game(5, "HUB_GAME_CRANEOPERATOR"),
            new Game(6, "HUB_GAME_CHASETHEBOAT"),
            new Game(7, "HUB_GAME_CHASETHETRUCK"),
            new Game(8, "HUB_GAME_PUSHTHECRATE"),
            new Game(9, "HUB_GAME_ZOOTRANSPORT"),
            new Game(35, "HUB_GAME_MOTIONCOMIC_ADVENTURE1_OUTRO")
        }),
        new Location(11, "HUB_LOCATION_ZOO", new Game[]
        {
            new Game(37, "HUB_GAME_MOTIONCOMIC_ADVENTURE2_INTRO"),
            new Game(4, "HUB_GAME_CONNECTTHEPIPES"),
            new Game(16, "HUB_GAME_BEATTHEBEAGLEBOYS"),
            new Game(27, "HUB_GAME_BATCAVE"),
            new Game(1, "HUB_GAME_JUMPTHECROC"),
            new Game(28, "HUB_GAME_MONKEYBATTLE"),
            new Game(38, "HUB_GAME_MOTIONCOMIC_ADVENTURE2_OUTRO")
        }),
        new Location(2, "HUB_LOCATION_GRANDMAS_FARM", new Game[]
        {
            new Game(10, "HUB_GAME_FRUITYARD")
        }),
        new Location(3, "HUB_LOCATION_GYROS_WORKSHOP", new Game[]
        {
            new Game(12, "HUB_GAME_ASSEMBLYLINE")
        }),
        new Location(6, "HUB_LOCATION_SCHOOL", new Game[]
        {
            new Game(23, "HUB_GAME_HIGHERTHAN"),
            new Game(24, "HUB_GAME_HANGMAN"),
            new Game(33, "HUB_GAME_TRANSLATETHIS")
        }),
        new Location(14, "HUB_LOCATION_STATION", new Game[]
        {
            new Game(30, "HUB_GAME_TRAINSPOTTING"),
            new Game(26, "HUB_GAME_CRATEMESS")
        }),
        new Location(15, "HUB_LOCATION_TVSTUDIO", new Game[]
        {
            new Game(36, "HUB_GAME_DUCKQUIZ")
        }),
        new Location(12, "HUB_LOCATION_SHOPPING_STREET", new Game[]
        {
            new Game(22, "HUB_GAME_FASHIONSTORE")
        }),
        new Location(5, "HUB_LOCATION_AVATAR_HOUSE", new Game[]
        {
            new Game(19, "HUB_GAME_WARDROBE")
        })
    };
}
