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
    
    // SkyBox
}
