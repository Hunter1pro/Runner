using System;
using System.Threading.Tasks;
using GameObjectService;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public enum PopupType { GameStart, GameWin, GameLost, GameEnd }
    public class UIEntry : BaseSystem, IDisposable
    {
        protected override int _initOrder { get; } = -1;

        [SerializeField] 
        private TopPanelView _topPanelView;

        [SerializeField] 
        private PopupView _popupView;
        
        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public void ShowPopup(PopupType popupType, UnityAction action)
        {
            _popupView.Button.onClick.RemoveAllListeners();
            _popupView.Panel.SetActive(true);
            
            switch (popupType)
            {
                case PopupType.GameStart:
                    _popupView.HeaderText.text = "Use Swipe Left & Right";
                    _popupView.ButtonText.text = "Start";
                    _popupView.Button.onClick.AddListener(action);
                    break;
                case PopupType.GameWin:
                    _popupView.HeaderText.text = "Game Win";
                    _popupView.ButtonText.text = "Next Level";
                    _popupView.Button.onClick.AddListener(action);
                    break;
                case PopupType.GameLost:
                    _popupView.HeaderText.text = "Game Lost";
                    _popupView.ButtonText.text = "Restart";
                    _popupView.Button.onClick.AddListener(action);
                    break;
                case PopupType.GameEnd:
                    _popupView.HeaderText.text = "Game End";
                    _popupView.ButtonText.text = "Start Again";
                    _popupView.Button.onClick.AddListener(action);
                    break;
            }
        }
        
        public void StartGame(int level)
        {
            _topPanelView.LevelText.text = $"Level {level+1}";
        }

        public void ScoreUpdate(int score)
        {
            _topPanelView.LevelText.text = $"Score {score}";
        }

        public void Dispose()
        {
            _topPanelView.LevelText.text = $"Level";
            _topPanelView.ScoreText.text = $"Score: 0";
            
            _popupView.Panel.SetActive(false);
            _popupView.Button.onClick.RemoveAllListeners();
        }
    }

}
