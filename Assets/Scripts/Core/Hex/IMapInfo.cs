using System.Collections.Generic;

namespace HexLib
{
    public interface IMapInfo
    {
        Dictionary<string, Hex> HexMap { get; }
    }
}
