using System.Collections.Generic;
using Game.Level.Views;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    [field: SerializeField] 
    public Material Material { get; private set; }

    [field: SerializeField] 
    public int Height { get; private set; } = 100;
    
    [field: SerializeField] 
    public int Weight { get; private set; } = 3;

    [field: SerializeField, Range(1, 300)] 
    public int NextObstacleDistance { get; private set; } = 15;
    [field: SerializeField, Range(1, 300)] 
    public int RandomRangeDistance { get; private set; } = 5;

    [field: SerializeField] 
    public List<TriggerSubscribe> ObstacleAssets { get; private set; }
    
    // SkyBox
}
