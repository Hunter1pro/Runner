using System;
using System.Collections.Generic;
using Game.Utils;
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

        private List<float3> _path = new List<float3>();
        private List<IMoveFinish> _moveFinished = new List<IMoveFinish>();

        public MoveComponent(CharacterAnim character, HexGridSystem hexGridSystem, float speed = 5)
        {
            _character = character;
            _hexGridSystem = hexGridSystem;
            _speed = speed;
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

        public void SubscribeFinish(IMoveFinish moveFinished)
        {
            _moveFinished.Add(moveFinished);
        }

        public override void Update()
        {
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

            if (IsMove && _pathStep >= _path.Count)
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
        }
    }
    
    public interface IMoveComponent : IDisposable
    {
        bool IsMove { get; }
        void SubscribeFinish(IMoveFinish moveFinished);
        void Move(List<float3> path);
    }

    public interface IMoveFinish
    {
        void MoveFinished(float3 position);
    }
}

