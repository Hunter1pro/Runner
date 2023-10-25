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

        public async Task<DIContainer> GenerateGameServices((LevelProvider levelProvider, Layout layout, Material material) levelProvider, 
            IDownloadBundle downloadBundle, HexGridSystem hexGridSystem, IMoveFinish moveFinish)
        {
            var characterAsset = await downloadBundle.DownloadAsset(_gameEntryView.CharacterAsset);
            var characterInstance = GameObject.Instantiate(characterAsset,
                hexGridSystem.HexToPosition(levelProvider.levelProvider.LevelData.StartCoordinate), Quaternion.identity);
            
            DIServiceCollection diServiceCollection = new();
            diServiceCollection.RegisterSingleton<ILevelObjectsContainer, LevelObjectsContainer>();
            diServiceCollection.RegisterSingleton<ICustomLogger, Logger>();
            diServiceCollection.RegisterSingleton<ILevelProvider, LevelProvider>(levelProvider.levelProvider);
            diServiceCollection.RegisterSingleton<IMapInfo, LevelProvider>(levelProvider.levelProvider);
            diServiceCollection.RegisterSingleton<IMoveComponent, MoveComponent>();
            diServiceCollection.RegisterSingleton(hexGridSystem);
            diServiceCollection.RegisterSingleton(characterInstance.GetComponent<CharacterAnim>());
            diServiceCollection.RegisterSingleton(levelProvider.layout);

            var container = diServiceCollection.GenerateContainer();

            var levelObjectsContainer = container.GetService<ILevelObjectsContainer>();
            var moveComponent = container.GetService<IMoveComponent>();

            levelObjectsContainer.AddLevelObject(characterInstance);

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

