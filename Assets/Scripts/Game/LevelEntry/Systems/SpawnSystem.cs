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
        private ILevelObjectsContainer _levelObjectsContainer;

        private GameObject _root;

        public SpawnSystem(ILevelObjectsContainer levelObjectsContainer)
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
            var instance = new GameObject(name);
            instance.name = name;
            _levelObjectsContainer.AddLevelObject(instance);
            
            return instance;
        }
    }

    public interface ISpawnSystem
    {
        [Obsolete("Can be AssetView base class")]
        T Spawn<T>(T asset, Transform root = null) where T : Object;

        GameObject SpawnEmpty(string name);
    }
}
