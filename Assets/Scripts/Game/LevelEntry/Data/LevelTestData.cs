using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "LevelTestData", menuName = "Level/LevelTestData")]
public class LevelTestData : ScriptableObject
{
    [field: SerializeField] 
    public Material Material { get; private set; }

    [field: SerializeField] 
    public int Height { get; private set; } = 100;
    
    [field: SerializeField] 
    public int Weight { get; private set; } = 3;
    
    [field: SerializeField] 
    public List<string> ObstacleAssets { get; private set; }
    
    [field: SerializeField]
    public string CoinAsset { get; private set; }
    
    [field: SerializeField]
    public string SpeedBonusAsset { get; private set; }
    
    [field: SerializeField, AssetReferenceUILabelRestriction]
    public List<string> SkyBoxes { get; private set; }
}
