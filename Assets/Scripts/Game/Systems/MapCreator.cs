using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameObjectService;
using HexLib;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class MapCreator : BaseSystem, IMapCreator
    {
        protected override int initOrder => -2;
        
        [field: SerializeField]
        public int Size {get; private set; } = 1;

        [SerializeField] private int _weight = 3;
        [SerializeField] private int _height = 100;
        
        [SerializeField]
        private Material _material;

        private GameObject _gameObject;
        private Mesh _mesh;

        private List<Vector3> _vertices = new List<Vector3>();
        private List<int> _triangles = new List<int>();
        private Layout _layout;
        
        public Dictionary<string, Hex> Map { get; private set; } = new Dictionary<string, Hex>();

        public override Task Init()
        {
            _layout = new Layout(Layout.Flat, Size, new float3(Size, 0, Size * Mathf.Sqrt(3) / 2));
            SpawnMap(0,_weight, 0, _height);
            return Task.CompletedTask;
        }

        public void Clear()
        {
            if (_gameObject)
                GameObject.Destroy(_gameObject);

            Map.Clear();
            _vertices.Clear();
            _triangles.Clear();
            _mesh.Clear();
        }

        public void SpawnMap(int left, int right, int top, int bottom)
        {
            _gameObject = new GameObject("HexMap");

            // Need Layout offset update for change root position
            _gameObject.transform.position = Vector3.zero;

            var meshRender = _gameObject.AddComponent<MeshRenderer>();
            meshRender.sharedMaterial = _material;

            _mesh = _gameObject.AddComponent<MeshFilter>().mesh;

            for (int r = top; r <= bottom; r++)
            {
                int r_offset = Mathf.FloorToInt(r / 2.0f);
                
                for (int q = left - r_offset; q <= right - r_offset; q++)
                {
                    var hex = new Hex(q, r, -q - r);
                    if (Map.Values.ToList().Exists(x => x == hex))
                        Debug.LogError($"Value is exist q {hex.Q} r {hex.R} s {hex.S}");
                    Map.Add(hex.CoordinateId, hex);
                    AddHex(hex);
                }
            }

            _mesh.vertices = _vertices.ToArray();
            _mesh.triangles = _triangles.ToArray();
            _mesh.RecalculateNormals();

            _gameObject.AddComponent<MeshCollider>().sharedMesh = _mesh;
        }

        private void AddHex(Hex h, float y = 0)
        {
            var corners = _layout.PolygonCorners(h);

            var center = _layout.HexToPixel(h);
            center.y = y;

            for (int i = 0; i < corners.Count - 1; i++)
            {
                AddTriangle(
                    center,
                     new float3((float)corners[i].x, y, (float)corners[i].z),
                     new float3((float)corners[i + 1].x, y, (float)corners[i + 1].z));
            }

            AddTriangle(
                    center,
                     new float3((float)corners[corners.Count - 1].x, y, (float)corners[corners.Count - 1].z),
                     new float3((float)corners[0].x, y, (float)corners[0].z));
        }

        private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            int vertexIndex = _vertices.Count;
            _vertices.Add(v1);
            _vertices.Add(v2);
            _vertices.Add(v3);
            _triangles.Add(vertexIndex);
            _triangles.Add(vertexIndex + 1);
            _triangles.Add(vertexIndex + 2);
        }
    }
}