using System;
using System.Collections.Generic;
using Game.Editor.Systems.Views;
using Game.Level.Data;
using Game.Level.Systems;
using Game.Utils;
using HexLib;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Game.Editor.Systems
{
    public class CurrentLevelContext : IDisposable
    {
        public LevelData CurrentLevel { get; set; }
        public GameObject CurrentMapObject { get; set; }

        public void Dispose()
        {
            GameObject.Destroy(CurrentMapObject);
        }
    }
    
    public enum SelectActionType { None, SpawnObstacles, StartPoint, EndPoint }
    
    public class LevelEditorService
    {
        private IMapCreator _mapCreator;
        private IDownloadBundle _downloadBundle;
        private EditorView _editorView;
        private ILevelCast _levelCast;
        [Obsolete("Split for game and Util like editor to make in dicontainer")]
        private HexGridSystem _hexGridSystem;
        private LevelContainerFile _levelContainerFile;
        private LevelDataContainer _levelDataContainer;
        private Layout _layout;

        private SelectActionType _selectActionType;

        private CurrentLevelContext _currentLevelContext;

        private PlayerInput playerInput;
        
        public LevelEditorService(IMapCreator mapCreator, IDownloadBundle downloadBundle, EditorView editorView, LevelDataContainer levelDataContainer, 
            ILevelCast levelCast, Layout layout, LevelContainerFile levelContainerFile)
        {
            _mapCreator = mapCreator;
            _downloadBundle = downloadBundle;
            _editorView = editorView;
            _levelCast = levelCast;
            _levelDataContainer = levelDataContainer;
            _layout = layout;
            _levelContainerFile = levelContainerFile;

            playerInput = new PlayerInput();

            playerInput.Player.Fire.performed += OnClick;
            playerInput.Enable();

            if (_levelDataContainer != null && _levelDataContainer.LevelDatas != null)
            {
                _levelDataContainer.LevelDatas.ForEach(levelData =>
                {
                    SpawnLevelButton($"Level_{_levelDataContainer.LevelDatas.IndexOf(levelData)}", () =>
                    {
                        LoadLevel(levelData);
                    }, button =>
                    {
                        RemoveLevel(levelData, button);
                    });
                });
            }
            else
            {
                _levelDataContainer = new LevelDataContainer { LevelDatas = new List<LevelData>() };
            }
            
            SpawnActionButton("Create", () =>
            {
                if (_currentLevelContext != null)
                    _currentLevelContext.Dispose();
                
                var currentLevelContext = new CurrentLevelContext();
                currentLevelContext.CurrentMapObject = new GameObject("HexMap");
                var testMap = _mapCreator.SpawnMap(currentLevelContext.CurrentMapObject, 0, _editorView.LevelTestData.Height, 0,
                    _editorView.LevelTestData.Weight, _editorView.LevelTestData.Material);
                
                currentLevelContext.CurrentLevel = new LevelData { HexMap = testMap };
                _levelDataContainer.LevelDatas.Add(currentLevelContext.CurrentLevel);
                _hexGridSystem = new HexGridSystem(_layout, new LevelProvider(currentLevelContext.CurrentLevel));
                
                _editorView.StartPoint.transform.position = _hexGridSystem.HexToPosition(currentLevelContext.CurrentLevel.StartCoordinate);
                _editorView.EndPoint.transform.position = _hexGridSystem.HexToPosition(currentLevelContext.CurrentLevel.EndCoordinate);
                
                SpawnLevelButton($"Level_{_levelDataContainer.LevelDatas.IndexOf(currentLevelContext.CurrentLevel)}", () =>
                {
                    LoadLevel(currentLevelContext.CurrentLevel);
                }, button =>
                {
                    RemoveLevel(currentLevelContext.CurrentLevel, button);
                });

                _currentLevelContext = currentLevelContext;
            });

            SpawnActionButton("None", () => { _selectActionType = SelectActionType.None; });
            SpawnActionButton("SpawnObstacles", () => { _selectActionType = SelectActionType.SpawnObstacles; });
            SpawnActionButton("StartPoint", () => { _selectActionType = SelectActionType.StartPoint;});
            SpawnActionButton("EndPoint", () => { _selectActionType = SelectActionType.EndPoint; });

            SpawnActionButton("Save", () =>
            {
                SaveSystem<LevelDataContainer>.Save(_levelContainerFile, _levelDataContainer);
            });
        }

        private async void OnClick(InputAction.CallbackContext value)
        {
            if (_currentLevelContext == null) return;
            
            var result = _levelCast.Touch(_editorView.Panel);

            if (result.exist is false) return;

            switch (_selectActionType)
            {
                case SelectActionType.StartPoint:
                    _editorView.StartPoint.transform.position = _hexGridSystem.GetHexPoint(result.hit.point);
                    _currentLevelContext.CurrentLevel.StartCoordinate = _hexGridSystem.GetHex(result.hit.point);
                    break;
                case SelectActionType.EndPoint:
                    _editorView.EndPoint.transform.position = _hexGridSystem.GetHexPoint(result.hit.point);
                    _currentLevelContext.CurrentLevel.EndCoordinate = _hexGridSystem.GetHex(result.hit.point);
                    break;
                case SelectActionType.SpawnObstacles:
                    if (result.exist)
                    {
                        var hex = _hexGridSystem.GetHex(result.hit.point);
                        var hexPosition = _hexGridSystem.GetHexPoint(result.hit.point);
                        var obstacles = _editorView.LevelTestData.ObstacleAssets;
                        var assetAddress = obstacles[Random.Range(0, obstacles.Count)];
                        var asset = await _downloadBundle.DownloadAsset(assetAddress);
                        var obstacle = GameObject.Instantiate(asset, hexPosition, Quaternion.identity, _currentLevelContext.CurrentMapObject.transform);
                        _currentLevelContext.CurrentLevel.ObstaclesDatas.Add(new ObstaclesData { AssetAddress = assetAddress, Coordinate = hex});
                    }
                    break;
            }
        }

        private void SpawnLevelButton(string name, Action open, Action<ButtonView> remove)
        {
            var buttonItem = GameObject.Instantiate(_editorView.ButtonItemViewPrefab, _editorView.LevelsPanelRoot);
            buttonItem.Subscribe(open);
            buttonItem.SubscribeClose(() => remove?.Invoke(buttonItem));
            buttonItem.SetText(name);
        }
        
        private void SpawnActionButton(string name, Action action)
        {
            var buttonView = GameObject.Instantiate(_editorView.ButtonViewPrefab, _editorView.ActionsPanelRoot);
            buttonView.Subscribe(action);
            buttonView.SetText(name);
        }

        private async void LoadLevel(LevelData levelData)
        {
            if (_currentLevelContext != null)
                _currentLevelContext.Dispose();
            
            _currentLevelContext = new CurrentLevelContext();
            _currentLevelContext.CurrentLevel = levelData;

            _hexGridSystem = new HexGridSystem(_layout,
                new LevelProvider(_currentLevelContext.CurrentLevel));

            _currentLevelContext.CurrentMapObject = 
                _mapCreator.SpawnMap(levelData.HexMap, _editorView.LevelTestData.Material);
            
            _editorView.StartPoint.transform.position = _hexGridSystem.HexToPosition(_currentLevelContext.CurrentLevel.StartCoordinate);
            _editorView.EndPoint.transform.position = _hexGridSystem.HexToPosition(_currentLevelContext.CurrentLevel.EndCoordinate);

            foreach (var obstacle in _currentLevelContext.CurrentLevel.ObstaclesDatas)
            {
                var asset = await _downloadBundle.DownloadAsset(obstacle.AssetAddress);
                var obstacleInstance = GameObject.Instantiate(asset, _hexGridSystem.HexToPosition(obstacle.Coordinate), 
                    Quaternion.identity, _currentLevelContext.CurrentMapObject.transform);
            }
        }
        
        private void RemoveLevel(LevelData currentLevel, ButtonView buttonView)
        {
            if (_currentLevelContext != null && _currentLevelContext.CurrentLevel == currentLevel)
            {
                _currentLevelContext.Dispose();
            }
            
            _levelDataContainer.LevelDatas.Remove(currentLevel);

            GameObject.Destroy(buttonView.gameObject);
        }
    }
}