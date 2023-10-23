using System.Collections.Generic;

namespace HexLib
{
    public interface IMapCreator
    {
        int Size  { get; }
        Dictionary<string, Hex> Map { get; }
    }
}
