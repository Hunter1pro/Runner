using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public static class SaveSystem<T>
    {
        public static T Load(string fileName, string foulder)
        {
            T loadPlayerData = SaveHelper.Load<T>(SaveHelper.Path(foulder), fileName);

            return loadPlayerData;
        }

        public static T Load(ISaveFile saveFile)
        {
            T loadPlayerData = SaveHelper.Load<T>(SaveHelper.Path(saveFile.FoulderName), saveFile.FileName);

            return loadPlayerData;
        }

        public static void Save(string fileName, string foulder, T playerData)
        {
            SaveHelper.Save(SaveHelper.Path(foulder), fileName, playerData);
        }

        public static void Save(ISaveFile saveFile, T playerData)
        {
            SaveHelper.Save(SaveHelper.Path(saveFile.FoulderName), saveFile.FileName, playerData);
        }
}
