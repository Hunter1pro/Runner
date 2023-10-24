using System.Threading.Tasks;
using DIContainerLib;
using Game.Editor.Systems;
using Game.Editor.Systems.Views;
using Game.Level.Data;
using Game.Level.Systems;
using Game.Utils;
using Game.Views;
using GameObjectService;
using HexLib;
using Unity.Mathematics;
using UnityEngine;
using Logger = Game.Utils.Logger;

namespace Game.Editor
{
    public class EditorEntry : BaseSystem
    {
        [SerializeField] 
        private EditorView _editorView;
    
        [SerializeField] 
        private LevelCastView _levelCastView;

        private LevelContainerFile _levelContainerFile = new LevelContainerFile();
        protected override int _initOrder { get; }

        public override Task Init()
        {
            DIServiceCollection diServiceCollection = new DIServiceCollection();
            
            var layout = new Layout(Layout.Flat, _editorView.Size, new float3(_editorView.Size, 0, _editorView.Size * Mathf.Sqrt(3) / 2));

            var levelContainerData = SaveSystem<LevelDataContainer>.Load(_levelContainerFile);
            if (levelContainerData == null)
                levelContainerData = new LevelDataContainer();
            
            diServiceCollection.RegisterSingleton<IMapCreator, MapCreator>();
            diServiceCollection.RegisterSingleton<ICustomLogger, Logger>();
            diServiceCollection.RegisterSingleton<ILevelCast, LevelCast>();
            diServiceCollection.RegisterSingleton<HexGridSystem>();
            diServiceCollection.RegisterSingleton<LevelEditorService>();
            diServiceCollection.RegisterSingleton<IDownloadBundle, DownloadBundle>();
            diServiceCollection.RegisterSingleton(_levelContainerFile);
            diServiceCollection.RegisterSingleton(_levelCastView);
            diServiceCollection.RegisterSingleton(_editorView);
            diServiceCollection.RegisterSingleton(layout);
            diServiceCollection.RegisterSingleton(levelContainerData);

            var diContaier = diServiceCollection.GenerateContainer();

            diContaier.GetService<LevelEditorService>();
        
            return Task.CompletedTask;;
        }
    }
}

