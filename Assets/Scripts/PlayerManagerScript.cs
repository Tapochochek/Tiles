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
        public int People { get; set; }
    }
    public PlayerResources playerResources;


    [SerializeField] private GameObject unitList;
    [SerializeField] private TextMeshProUGUI[] resources;

    void Start()
    {
        playerResources.Wood = 0;
        playerResources.Stone = 0;
        playerResources.Metal = 0;

        SaveResources();
        LoadResources();
    }

    public void SetStartResourceValues()
    {
        playerResources.Wood = 0;
        playerResources.Stone = 0;
        playerResources.Metal = 0;
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
            resources[0].text = playerResources.Wood.ToString();
            resources[1].text = playerResources.Stone.ToString();
            resources[2].text = playerResources.Metal.ToString();
            resources[3].text = playerResources.People.ToString();
        }
        else
        {
            Debug.LogError("Player resources file not found!");
        }
    }
    public void AddResources(int resources, string resource)
    {
        if (resource == "Wood")
        {
            playerResources.Wood += resources;          
        }
        else if (resource == "Stone")
        {
            playerResources.Stone += resources;
        }
        else if (resource == "Metal")
        {
            playerResources.Metal += resources;
        }
        SaveResources();
        LoadResources();
    }
    public void AddPeopls(int countAdd)
    {
        playerResources.People += countAdd;

        SaveResources();
        LoadResources();
    }
    public void ButtonUnitsClick() { 
        unitList.SetActive(!unitList.activeSelf);
    }
}
