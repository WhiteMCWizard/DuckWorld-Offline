using System;

namespace SLAM.SaveSystem
{
    public interface ISaveDataProvider
    {
        void Save(SaveData data);
        SaveData Load();
    }
}
