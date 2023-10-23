using System;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Level.Systems
{
    /// <summary>
    /// Dispose is called when DIContainer is disposed
    /// </summary>
    public class LevelObjectsContainer : ILevelObjectsContainer
    {
        private List<Object> _levelAssets = new();
        private ICustomLogger _logger;

        public LevelObjectsContainer(ICustomLogger logger)
        {
            _logger = logger;
        }
        
        public void AddLevelObject(Object gameObject)
        {
            _levelAssets.Add(gameObject);
        }

        public void Dispose()
        {
            _levelAssets.ForEach(levelAsset =>
            {
                if (levelAsset is not null)
                {
                    Object.Destroy(levelAsset);
                }
                else
                {
                    _logger.LogError($"{nameof(LevelObjectsContainer)} level asset already destroyed");
                }
            });
        }
    }

    public interface ILevelObjectsContainer : IDisposable
    {
        void AddLevelObject(Object gameObject);
    }
}
