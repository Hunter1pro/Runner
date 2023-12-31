using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexLib
{
    public class GetNeigboursHex : IGetNeigbours<Hex>
    {
        private List<Hex> _map;
        private bool _logging = false;

        public GetNeigboursHex(List<Hex> map)
        {
            _map = map;
        }

        public int CalculateDistance(Hex from, Hex To)
        {
            return from.Distance(To);
        }

        public List<Hex> GetNeighbours(Hex point)
        {
            List<Hex> neighbours = new List<Hex>();

            foreach(var hex in Hex.Dirrections)
            {
                var newHex = point.Add(hex);

                try
                {
                    var mapHex = _map.First(x => x == newHex);

                    neighbours.Add(mapHex);
                }
                catch
                {
                    Logging("Out Of map");
                }
            }

            return neighbours;
        }

        private void Logging(string value)
        {
            if (_logging)
                Debug.Log(value);
        }
    }
}


