using UnityEngine;
using Newtonsoft.Json;
using System.IO;
public class PlayerManagerScript : MonoBehaviour
{
    [System.Serializable]
    public class PlayerResources
    {
        public int Wood { get; set; }
        public int Stone { get; set; }
        public int Metal { get; set; }
    }
    public PlayerResources playerResources;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerResources.Wood = 100;
        playerResources.Stone = 100;
        playerResources.Metal = 100;
        SaveResources();
        LoadResources();
    }
    public void SaveResources()
    {
        string json = JsonConvert.SerializeObject(playerResources);
        string path = Path.Combine(Application.persistentDataPath, "playerResources.json");
        File.WriteAllText(path, json);
    }
    public void LoadResources()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerResources.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            playerResources = JsonConvert.DeserializeObject<PlayerResources>(json);
            Debug.Log("Wood: " + playerResources.Wood);
            Debug.Log("Stone: " + playerResources.Stone);
            Debug.Log("Metal: " + playerResources.Metal);
        }
        else
        {
            Debug.LogError("Player resources file not found!");
        }
    }
}
