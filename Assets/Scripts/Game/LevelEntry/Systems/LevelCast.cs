using Game.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Level.Systems
{
    public class LevelCast : ILevelCast
    {
        private Camera _camera;

        private LayerMask _layerMask;

        private float _distance = 500;

        public LevelCast(LevelCastView levelCastView)
        {
            _camera = levelCastView.Camera;
            _layerMask = levelCastView.LayerMask;
            _distance = levelCastView.Distance;
        }

        public (bool exist, RaycastHit hit) Touch(Ray ray)
        {
            RaycastHit hitInfo = default;
            bool result = Physics.Raycast(ray, out hitInfo, _distance, _layerMask);
            return (result, hitInfo);
        }
    
        public (bool exist, RaycastHit hit) Touch()
        {
            RaycastHit hitInfo = default;
            bool result = Physics.Raycast(_camera.ScreenPointToRay(MouseScreenPosition()), out hitInfo, _distance, _layerMask);
            return (result, hitInfo);
        }

        public (bool exist, RaycastHit hit) Touch(RectTransform ignorePanel)
        {
            RaycastHit hitInfo = default;
            var mousePos = Input.mousePosition;
            bool result = false;
            
            if (IsOnPanel.Check(ignorePanel, Input.mousePosition) is false)
            {
                result = Physics.Raycast(_camera.ScreenPointToRay(mousePos), out hitInfo, _distance, _layerMask);
            }
            
            return (result, hitInfo);
        }

        public Vector3 MouseScreenPosition()
            => new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, _camera.nearClipPlane);
    }

    public interface ILevelCast
    {
        (bool exist, RaycastHit hit) Touch(Ray ray);
        (bool exist, RaycastHit hit) Touch();
        (bool exist, RaycastHit hit) Touch(RectTransform ignorePanel);
        Vector3 MouseScreenPosition();
    }
}

