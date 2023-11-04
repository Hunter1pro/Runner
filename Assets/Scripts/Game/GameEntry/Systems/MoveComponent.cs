using System.Collections.Generic;
using Game.Utils;
using Game.Views;
using HexLib;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Systems
{
    public class MoveComponent : Updatable, IMoveComponent
    {
        private CharacterAnim _character;
        private HexGridSystem _hexGridSystem;

        private int _pathStep;
        public bool IsMove { get; private set; }
        private float _speed;

        private float _entrySpeed;
        
        private List<float3> _path = new List<float3>();
        private List<IMoveFinish> _moveFinished = new List<IMoveFinish>();
        
        public MoveComponent(CharacterAnim character, HexGridSystem hexGridSystem, GameEntryView gameEntryView, ISwipeInput swipeInput)
        {
            _character = character;
            _hexGridSystem = hexGridSystem;

            _speed = gameEntryView.Speed;
            _entrySpeed = _speed;
            
            swipeInput.SwipeAction += SwipeInput;
        }

        private void SwipeInput(bool right)
        {
            ChangeDirrection(right);
        }

        private void ChangeDirrection(bool right)
        {
            if (IsMove is false || _path.Count == 0) return;
            
            if (right)
            {
                var currentHex = _hexGridSystem.GetHex(_path[_pathStep]);
                var nextHex = currentHex.Neighbour(2);
                var finishHex = _hexGridSystem.GetHex(_path[_path.Count - 1]);
                var newFinishHex = finishHex.Neighbour(2);

                if (_hexGridSystem.ExistInMap(nextHex) && _hexGridSystem.ExistInMap(newFinishHex))
                {
                    _path = _hexGridSystem.GetPath(nextHex, newFinishHex);
                    _pathStep = 0;
                }
            }
            else
            {
                var currentHex = _hexGridSystem.GetHex(_path[_pathStep]);
                var nextHex = currentHex.Neighbour(5);
                var finishHex = _hexGridSystem.GetHex(_path[_path.Count - 1]);
                var newFinishHex = finishHex.Neighbour(5);
                
                if (_hexGridSystem.ExistInMap(nextHex) && _hexGridSystem.ExistInMap(newFinishHex))
                {
                    _path = _hexGridSystem.GetPath(nextHex, newFinishHex);
                    _pathStep = 0;
                }
            }
        }

        public void Move(List<float3> path)
        {
            _path = path;

            if (_path != null && _path.Count > 0)
            {
                IsMove = true;
            }
            else
            {
                Debug.LogError("Path is null");
            }

        }

        public void UpdateSpeed(float value)
        {
            _speed = value;
        }

        public void RestoreSpeed()
        {
            _speed = _entrySpeed;
        }

        public void SubscribeFinish(IMoveFinish moveFinished)
        {
            _moveFinished.Add(moveFinished);
        }

        public override void Update()
        {
            if (_character == null) return;
            
            if (IsMove && _pathStep < _path.Count)
            {
                _character.PlayRun();
                Vector3 destination = _path[_pathStep];
                destination.y = _character.transform.position.y;
                _character.transform.position = Vector3.Lerp(_character.transform.position, destination,
                    1 / Vector3.Distance(_character.transform.position, destination) * _speed * Time.deltaTime);

                _character.transform.LookAt(destination);

                if (Vector3.Distance(_character.transform.position, destination) < 0.01f)
                    _pathStep++;
            }

            if (IsMove && _pathStep >= _path.Count && _path.Count > 0)
            {
                _character.PlayIdle();
                var finalPoint = _path[_path.Count - 1];
                var hexWorldPoint = _hexGridSystem.GetHexPoint(finalPoint);
                hexWorldPoint.y = _character.transform.position.y;
                _character.transform.LookAt(hexWorldPoint);

                IsMove = false;
                _pathStep = 0;

                _moveFinished.ForEach(x => x.MoveFinished(finalPoint));

                _path.Clear();
            }

            if (IsMove && _path.Count == 0)
            {
                IsMove = false;
                _pathStep = 0;
                var finalPoint = _hexGridSystem.GetHexPoint(_character.transform.position);
                
                _moveFinished.ForEach(x => x.MoveFinished(finalPoint));

                _path.Clear();
            }
        }
    }
    
    public interface IMoveComponent
    {
        bool IsMove { get; }
        void SubscribeFinish(IMoveFinish moveFinished);
        void Move(List<float3> path);

        void UpdateSpeed(float value);
        void RestoreSpeed();
    }

    public interface IMoveFinish
    {
        void MoveFinished(float3 position);
    }
}

