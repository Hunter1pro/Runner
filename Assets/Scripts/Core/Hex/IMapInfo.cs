using System.Collections.Generic;

namespace HexLib
{
    public interface IMapInfo
    {
        int Size  { get; }
        Dictionary<string, Hex> Map { get; }
    }
}
