using System;
using UnityEngine;

[Serializable]
public class MapCreatorView
{
    [field: SerializeField]
    public int Size {get; private set; } = 1;
}
