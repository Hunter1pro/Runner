
using System;
using Game.Utils;
using Game.Views;
using UnityEngine;

namespace Game.Systems
{
    public class SwipeInput : Updatable, ISwipeInput
    {
        private float _swipeScreenRatio;
        
        private bool _fingerDown;
        private Vector3 _startPosition;

        public event Action<bool> SwipeAction;

        public SwipeInput(GameEntryView gameEntryView)
        {
            _swipeScreenRatio = gameEntryView.SwipeScreenRatio;
        }
        
        public override void Update()
        {
            if (_fingerDown is false && Input.GetMouseButtonDown(0))
            {
                _fingerDown = true;
                _startPosition = Input.mousePosition;
            }
        
            if (_fingerDown && Input.mousePosition.x >= _startPosition.x + Screen.width * _swipeScreenRatio)
            {
                _fingerDown = false;
                SwipeAction(true);
            }
            else if (_fingerDown && Input.mousePosition.x <= _startPosition.x - Screen.width * _swipeScreenRatio)
            {
                _fingerDown = false;
                SwipeAction(false);
            }
        }
    }

    public interface ISwipeInput
    {
        event Action<bool> SwipeAction;
    }
}
