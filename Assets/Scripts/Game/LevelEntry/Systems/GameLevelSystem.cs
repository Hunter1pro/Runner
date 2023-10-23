using System.Collections.Generic;
using Game.Level.Views;
using HexLib;

namespace Game.Level.Systems
{
    public class GameLevelSystem : IGameLevelSystem
    {
        private IMapCreator _mapCreator;
        private List<LevelData> _levelDatas;
        
        // List possible levels
        // SaveSystem
        // Current Level
        // Next Level
        // Level Props kill/damage
        // Level Bonus Touch

        public GameLevelSystem(GameLevelView gameLevelView, IMapCreator mapCreator)
        {
            _levelDatas = gameLevelView.LevelDatas;
            _mapCreator = mapCreator;
        }
    }

    public interface IGameLevelSystem
    {
    }
}

