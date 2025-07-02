using System;
using SLAM.Achievements;
using SLAM.Webservices;

namespace SLAM.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public UserProfile profile = new UserProfile();
        public PlayerAvatarData avatar = new PlayerAvatarData();
        public UserGameDetails[] userGameDetails = new UserGameDetails[0];
        public UserAchievement[] userAchievements = new UserAchievement[0];
        public Message[] messages = new Message[0];
        public PurchasedShopItemData[] purchasedShopItems = new PurchasedShopItemData[0];
        public int walletTotal = 0;
        // Add more fields here as needed for future data
    }
}