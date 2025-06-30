using System;

namespace SLAM.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public Profile profile = new Profile();
        // Add more fields here as needed for future data
    }

    [Serializable]
    public class Profile
    {
        public int id;
        public string name;
        public string address;
        public bool is_free;
        public bool is_sa;
        public string mugshot;
    }
}
