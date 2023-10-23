using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace HexLib
{
    // Spawn or create components and objects in runtime
    public class HexGridSystem : IDisposable
    {
        private float _size;
        private Layout _layout;

        private HexPathfinding<Hex> _pathfinding;

        private Dictionary<string, Hex> _map = new Dictionary<string, Hex>();

        public HexGridSystem(IMapInfo mapInfo)
        {
            _size = mapInfo.Size;
            _map = mapInfo.Map;

            _layout = new Layout(Layout.Flat, _size, new float3(_size, 0, _size * Mathf.Sqrt(3) / 2));

            var neighbours = new GetNeigboursHex(_map.Values.ToList());

            _pathfinding = new HexPathfinding<Hex>(neighbours);
        }

        public float3 GetHexPoint(float3 hit)
        {
            var hex = _layout.PixelToHex(hit).HexRound();

            var point = _layout.HexToPixel(hex);

            return point;
        }

        public Hex GetHexByHash(string hash) => _map[hash];

        public float3 GetHexPoint(string hash) => HexToPosition(GetHexByHash(hash));

        public float3 HexToPosition(Hex hex)
        {
            var point = _layout.HexToPixel(hex);

            return point;
        }

        public Hex GetHex(float3 hit)
        {
            var hex = _layout.PixelToHex(hit).HexRound();

            return hex;
        }

        public Hex GetHex(int q, int r, int s)
        {
            var hex = new Hex(q, r, s);

            if (_map.TryGetValue(hex.CoordinateId, out var result))
            {
                return result;
            }

            throw new NullReferenceException($"Hex in coordinates {q} {r} {s} not found");
        }

        public List<float3> GetPath(float3 from, float3 to)
        {
            var hex = _layout.PixelToHex(to).HexRound();

            var point = _layout.HexToPixel(hex);

            var pathResult = _pathfinding.FindPath(_map.Values.FirstOrDefault(x => x == _layout.PixelToHex(from).HexRound()), _map.FirstOrDefault(x => x.Value == hex).Value, _map.Values.ToList());
            if (pathResult != null)
            {
                for (int i = 0; i < pathResult.Count - 1; i++)
                {
                    Debug.DrawLine(_layout.HexToPixel(pathResult[i]), _layout.HexToPixel(pathResult[i + 1]), Color.blue, 5);
                }
                Debug.Log($"PathResult: {pathResult.Count}");

                return pathResult.Select(x => _layout.HexToPixel(x)).ToList();
            }

            throw new System.Exception("Path not found");
        }

        public List<Hex> GetHexAround(float3 position, int range)
        {
            List<Hex> result = new List<Hex>();

            var point = GetHex(position);

            for(int q = -range; q <= range; q++)
            {
                for(int r = math.max(-range, -q -range); r <= math.min(range, -q + range); r++)
                {
                    var s = -q-r;
                    result.Add(point.Add(new Hex(q, r, s)));
                }
            }

            return result;
        }

        public void Dispose()
        {
            _map.Clear();
        }
    }
}

