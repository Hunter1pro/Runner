using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.UnityConverters;
using Newtonsoft.Json.UnityConverters.Math;
using UnityEngine;

    public static class SaveHelper
    {
        public static string Path(string foulder)
        {
            if (Application.isEditor)
            {
                string path = Application.dataPath + $"../../SaveData/{foulder}/";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
            else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string path = Application.streamingAssetsPath;

                return path;
            }
            else
            {
                string path = $"SaveData/{foulder}/";

                if (!Directory.Exists(foulder))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

    public static void Save<T>(string path, string fileName, T saveObject)
    {
        File.WriteAllText(System.IO.Path.Combine(path, fileName), Serialize(saveObject));
    }

    public static T Load<T>(string path, string filename)
    {
        try
        {
            string file = File.ReadAllText(System.IO.Path.Combine(path, filename));

            if (!string.IsNullOrEmpty(file))
                return Deserialize<T>(file);
            return default;
        }
        catch (FileNotFoundException ex)
        {
            return default;
        }
        catch (IOException ex)
        {
            Debug.Log($"{ex.Message}");
            return default;
        }
    }

    public static string Serialize<T>(T saveObject)
    {
        var jsonConfigSettings = new JsonSerializerSettings
        {
            Converters = new[] {
            new Vector3Converter(),
            },
            ContractResolver = new UnityTypeContractResolver(),
        };
        return JsonConvert.SerializeObject(saveObject, jsonConfigSettings);
    }

    public static T Deserialize<T>(string jsonString)
    {
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
}
