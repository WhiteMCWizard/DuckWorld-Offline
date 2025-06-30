using System;

namespace SLAM.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public UserProfile profile = new UserProfile();
        public PlayerAvatarData avatar = new PlayerAvatarData();
        // Add more fields here as needed for future data
    }
}