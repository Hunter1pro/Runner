using System;
using UnityEngine;

[Serializable]
public class MapCreatorView
{
    [field: SerializeField]
    public int Size {get; private set; } = 1;

    [field: SerializeField] 
    public int Weight { get; private set; } = 3;
    
    [field: SerializeField] 
    public int Height { get; private set; } = 100;
        
    [field: SerializeField]
    public Material Material { get; private set; }
}
