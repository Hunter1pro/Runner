using System.Threading.Tasks;
using DIContainerLib;
using Game.Level.Systems;
using Game.Systems;
using Game.Utils;
using Game.Views;
using GameObjectService;
using HexLib;
using UnityEngine;
using Logger = Game.Utils.Logger;

namespace Game
{
    public class GameEntry : BaseSystem
    {
        protected override int _initOrder { get; }

        [SerializeField] 
        private GameEntryView _gameEntryView;
        
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public async Task<DIContainer> GenerateGameServices((LevelProvider levelProvider, Layout layout, Material material) levelProvider, IMoveFinish moveFinish)
        {
            // Load Charcter
            // Attach Camera
            // Spawn to Start
            // Pathfinding to End
            // Move Thought Pathfinding
            // Trigger Finish
            // Trigger Obstacle
            // Jump

            DIServiceCollection diServiceCollection = new();
            diServiceCollection.RegisterSingleton<IDownloadBundle, DownloadBundle>();
            diServiceCollection.RegisterSingleton<ILevelObjectsContainer, LevelObjectsContainer>();
            diServiceCollection.RegisterSingleton<ICustomLogger, Logger>();
            diServiceCollection.RegisterSingleton<HexGridSystem>();
            diServiceCollection.RegisterSingleton<ILevelProvider, LevelProvider>(levelProvider.levelProvider);
            diServiceCollection.RegisterSingleton<IMapInfo, LevelProvider>(levelProvider.levelProvider);
            diServiceCollection.RegisterSingleton(levelProvider.layout);

            var container = diServiceCollection.GenerateContainer();

            var downloadBundle = container.GetService<IDownloadBundle>();
            var hexGridSystem = container.GetService<HexGridSystem>();
            var levelObjectsContainer = container.GetService<ILevelObjectsContainer>();

            var characterAsset = await downloadBundle.DownloadAsset(_gameEntryView.CharacterAsset);
            var characterInstance = GameObject.Instantiate(characterAsset,
                hexGridSystem.HexToPosition(levelProvider.levelProvider.LevelData.StartCoordinate), Quaternion.identity);
            
            levelObjectsContainer.AddLevelObject(characterInstance);

            var moveComponent = new MoveComponent(characterInstance.GetComponent<CharacterAnim>(), hexGridSystem);

            var path = hexGridSystem.GetPath(hexGridSystem.HexToPosition(levelProvider.levelProvider.LevelData.StartCoordinate),
                hexGridSystem.HexToPosition(levelProvider.levelProvider.LevelData.EndCoordinate));
            
            moveComponent.Move(path);
            moveComponent.SubscribeFinish(moveFinish);
            
            _gameEntryView.VirtualCamera.Follow = characterInstance.transform;
            _gameEntryView.VirtualCamera.LookAt = characterInstance.transform;

            return container;
        }
    }
}

