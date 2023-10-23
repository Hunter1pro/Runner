using System.Threading.Tasks;
using GameObjectService;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class LevelCast : BaseSystem
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private float _distance = 500;

        protected override int initOrder => 0;

        public override Task Init()
        {
            return Task.CompletedTask;
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
            bool result = Physics.Raycast(Camera.main.ScreenPointToRay(MouseScreenPosition()), out hitInfo, _distance, _layerMask);
            return (result, hitInfo);
        }
        public Vector3 MouseScreenPosition()
            => new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, Camera.main.nearClipPlane);
    }
}

