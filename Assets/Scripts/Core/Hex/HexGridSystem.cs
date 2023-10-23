using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace HexLib
{
    // Spawn or create components and objects in runtime
    public class HexGridSystem : IDisposable
    {
        [Obsolete("Layout Dublicate with MapCreator")]
        private Layout _layout;
        [Obsolete("Remove, make method lifetime")]
        private IMapInfo _mapInfo;

        // !Move to service lifetime
        // private HexPathfinding<Hex> _pathfinding;
        
        [Obsolete("Not Exist on Creator lifetime move to method lifetime^")]
        private Dictionary<string, Hex> _map = new Dictionary<string, Hex>();

        public HexGridSystem(IMapInfo mapInfo)
        {
            var size = mapInfo.Size;
            //_map = mapInfo.Map;
            _mapInfo = mapInfo;
            _layout = new Layout(Layout.Flat, size, new float3(size, 0, size * Mathf.Sqrt(3) / 2));

            // var neighbours = new GetNeigboursHex(_map.Values.ToList());
            //
            // _pathfinding = new HexPathfinding<Hex>(neighbours);
        }

        public float3 GetHexPoint(float3 hit)
        {
            var hex = _layout.PixelToHex(hit).HexRound();

            var point = _layout.HexToPixel(hex);

            return point;
        }

        public Hex GetHexByHash(string hash) => _mapInfo.Map[hash];

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

        public (Hex hex, bool found) GetHex(int q, int r, int s)
        {
            var hex = new Hex(q, r, s);
            
            Debug.Log($"#GetHex Count{_mapInfo.Map.Count} {hex}");
            if (_mapInfo.Map.TryGetValue(hex.CoordinateId, out var result))
            {
                return (result, true);
            }

            return (hex, false);
        }

        public List<float3> GetPath(float3 from, float3 to)
        {
            var hex = _layout.PixelToHex(to).HexRound();

            var point = _layout.HexToPixel(hex);

            // var pathResult = _pathfinding.FindPath(_map.Values.FirstOrDefault(x => x == _layout.PixelToHex(from).HexRound()), _map.FirstOrDefault(x => x.Value == hex).Value, _map.Values.ToList());
            // if (pathResult != null)
            // {
            //     for (int i = 0; i < pathResult.Count - 1; i++)
            //     {
            //         Debug.DrawLine(_layout.HexToPixel(pathResult[i]), _layout.HexToPixel(pathResult[i + 1]), Color.blue, 5);
            //     }
            //     Debug.Log($"PathResult: {pathResult.Count}");
            //
            //     return pathResult.Select(x => _layout.HexToPixel(x)).ToList();
            // }

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

        public bool ExistInMap(Hex hex)
        {
            return _mapInfo.Map.ContainsKey(hex.ToString());
        }

        public void Dispose()
        {
            _mapInfo.Map.Clear();
        }
    }
}

