using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace HexLib
{
    public class HexGridSystem
    {
        private Layout _layout;
        private IMapInfo _mapInfo;
        private HexPathfinding<Hex> _pathfinding;
        
        public HexGridSystem(Layout layout, IMapInfo mapInfo)
        {
            _layout = layout;
            _mapInfo = mapInfo;

            var neighbours = new GetNeigboursHex(mapInfo.HexMap.Values.ToList());
            
            _pathfinding = new HexPathfinding<Hex>(neighbours);
        }

        public float3 GetHexPoint(float3 hit)
        {
            var hex = _layout.PixelToHex(hit).HexRound();

            var point = _layout.HexToPixel(hex);

            return point;
        }

        public Hex GetHexByHash(string hash) => _mapInfo.HexMap[hash];

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
            
            if (_mapInfo.HexMap.TryGetValue(hex.CoordinateId, out var result))
            {
                return (result, true);
            }

            return (hex, false);
        }

        public List<float3> GetPath(float3 from, float3 to)
        {
            var hex = _layout.PixelToHex(to).HexRound();

            var map = _mapInfo.HexMap;

            var pathResult = _pathfinding.FindPath(map.Values.FirstOrDefault(x => x == _layout.PixelToHex(from).HexRound()), 
                map.FirstOrDefault(x => x.Value == hex).Value, map.Values.ToList());
            
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

        public bool ExistInMap(Hex hex, Dictionary<string, Hex> map)
        {
            return map.ContainsKey(hex.ToString());
        }
    }
}

