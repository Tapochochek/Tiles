using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using TMPro;
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
    [SerializeField] private GameObject unitList;
    [SerializeField] private TextMeshProUGUI[] resources;

    void Start()
    {
        playerResources.Wood = 100;
        playerResources.Stone = 100;
        playerResources.Metal = 100;
        SaveResources();
        LoadResources();
    }

    public void SetStartResourceValues()
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
        string path = Path.Combine(Application.persistentDataPath, $"playerResources{TurnManagerScript.currentTurn}.json");
        Debug.Log("Saving resources to: " + path);
        File.WriteAllText(path, json);
    }
    public void LoadResources()
    {
        string path = Path.Combine(Application.persistentDataPath, $"playerResources{TurnManagerScript.currentTurn}.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            playerResources = JsonConvert.DeserializeObject<PlayerResources>(json);

            Debug.Log("Wood: " + playerResources.Wood);
            resources[0].text = playerResources.Wood.ToString();
            Debug.Log("Stone: " + playerResources.Stone);
            resources[1].text = playerResources.Stone.ToString();
            Debug.Log("Metal: " + playerResources.Metal);
            resources[2].text = playerResources.Metal.ToString();
        }
        else
        {
            Debug.LogError("Player resources file not found!");
        }
    }
    public void ButtonUnitsClick() { 
        unitList.SetActive(!unitList.activeSelf);
    }
}
