using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Level.Systems
{
    /// <summary>
    /// Spawning all level object
    /// </summary>
    public class SpawnSystem : ISpawnSystem
    {
        private LevelObjectsContainer _levelObjectsContainer;

        private GameObject _root;

        public SpawnSystem(LevelObjectsContainer levelObjectsContainer)
        {
            _levelObjectsContainer = levelObjectsContainer;
        }
        public T Spawn<T>(T asset, Transform root = null) where T : Object
        {
            var instance = Object.Instantiate<T>(asset, root);
            
            _levelObjectsContainer.AddLevelObject(instance);
            
            return instance;
        }
        
        public GameObject SpawnEmpty(string name)
        {
            var instance = GameObject.Instantiate<GameObject>(null);
            instance.name = name;
            _levelObjectsContainer.AddLevelObject(instance);
            
            return instance;
        }
    }

    public interface ISpawnSystem
    {
        [Obsolete("Can be AssetView base class")]
        T Spawn<T>(T asset, Transform root = null) where T : Object;

        public GameObject SpawnEmpty(string name);
    }
}
