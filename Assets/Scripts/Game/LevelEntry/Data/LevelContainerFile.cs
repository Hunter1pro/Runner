
namespace Game.Level.Data
{
    public class LevelContainerFile : ISaveFile
    {
        public string FileName { get; } = "LevelContainer.json";
        public string AssetFile { get; } = "LevelContainer";
        public string FoulderName { get; } = "Editor";
    }
}

