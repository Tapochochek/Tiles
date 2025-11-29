using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using static PlayerManagerScript;

public class PeopleManageScript : MonoBehaviour
{
    [System.Serializable]
    public class FortressPeople
    {
        public int People { get; set; }
    }
    public FortressPeople fortressPeople;

    [SerializeField]
    private GameObject[] unitPrefabs;
    [SerializeField]
    private GameObject spawnUI;
    [SerializeField]
    private GameObject buildUI;
    public static GameObject selectedFortress;
    public bool isBuild;
    private string fortressId;
    PlayerManagerScript playerManager;
    TextMeshProUGUI peopleCountText;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManagerScript>();
        GetComponentInChildren<Canvas>().enabled = true;
        if (string.IsNullOrEmpty(fortressId))
        {
            peopleCountText = GetComponentInChildren<TextMeshProUGUI>();
            fortressPeople.People = 1;
            fortressId = System.Guid.NewGuid().ToString();

            SaveResources();
            LoadResources();
        }
    }
    public void SaveResources()
    {
        string json = JsonConvert.SerializeObject(fortressPeople);

        string fileName = $"fortressPeople_{fortressId}.json";

        string path = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log("Saving resources to: " + path);
        File.WriteAllText(path, json);
    }
    public void LoadResources()
    {
        string fileName = $"fortressPeople_{fortressId}.json";
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            fortressPeople = JsonConvert.DeserializeObject<FortressPeople>(json);
            peopleCountText.text = fortressPeople.People.ToString();
        }
        else
        {
            Debug.LogError("Player resources file not found!");
        }
    }

    public void AddPeople(int amount)
    {
        fortressPeople.People += amount;
        peopleCountText.text = fortressPeople.People.ToString();
        SaveResources();
    }

    public IEnumerator ShowFortressUI()
    {
        ClickLogick.isFortressUI = true;
        Canvas canvas = GameObject.Find("FortressUI").GetComponentInChildren<Canvas>();
        canvas.enabled = true;
        canvas.transform.Find("Attack").GetComponent<Button>().onClick.AddListener(PaintAttackRadius);
        canvas.transform.Find("Defend").GetComponent<Button>().onClick.AddListener(PaintDefendsRadius);
        canvas.transform.Find("Spawn").GetComponent<Button>().onClick.AddListener(SpawnUIOpen);
        canvas.transform.Find("Build").GetComponent<Button>().onClick.AddListener(BuildingUIOpen);
        while (true)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                break;
            }
            yield return null;
        }
        HideFortressUI();

    }
    public void HideFortressUI()
    {
        spawnUI.SetActive(false);
        ClickLogick.isFortressUI = false;
        Canvas canvas = GameObject.Find("FortressUI").GetComponentInChildren<Canvas>();
        canvas.enabled = false;

        try
        {
            GameObject canvasUnit = GameObject.Find("SpawnUI");
            canvasUnit.SetActive(false);
        }
        catch
        {
            Debug.Log("No SpawnUI activate");
        }

        try
        {
            GameObject canvasBuild = GameObject.Find("BuildingUI");
            canvasBuild.SetActive(false);
        }
        catch
        {
            Debug.Log("No BuildingUI activate");
        }

        
    }

    private void PaintAttackRadius()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<GameObject> attackZone = new List<GameObject>();
        selectedFortress = this.gameObject;

        foreach (var obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer(TurnManagerScript.currentTurn) && obj.tag == "Tile")
            {
                attackZone.Add(obj);
            }
        }
        foreach (var tile in GlobalContainer.trueAllTiles)
        {
            foreach (var attack in attackZone)
            {
                float distance = Vector3.Distance(attack.transform.position, tile.transform.position);
                if (distance <= GlobalContainer.hexRadius && tile.layer != LayerMask.NameToLayer(TurnManagerScript.currentTurn))
                {
                    tile.GetComponent<ClickLogick>().SelectedMultiply(tile);
                }
            }
        }
        HideFortressUI();
    }
    private void PaintDefendsRadius()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<GameObject> currentPlayerTiles = new List<GameObject>();
        selectedFortress = this.gameObject;
        foreach (var obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer(TurnManagerScript.currentTurn) && obj.tag == "Tile")
            {
                currentPlayerTiles.Add(obj);
            }
        }
        foreach (var tile in currentPlayerTiles)
        {
            tile.GetComponent<ClickLogick>().SelectedMultiply(tile);
        }
        HideFortressUI();
    }

    public void BuildingUIOpen()
    {
        HideFortressUI();
        selectedFortress = this.gameObject;
        buildUI.SetActive(true);
        buildUI.transform.Find("Wall").GetComponent<Button>().onClick.AddListener(Build);
        buildUI.transform.Find("Tower").GetComponent<Button>().onClick.AddListener(Build);

    }
    public void SpawnUIOpen()
    {
        HideFortressUI();
        selectedFortress = this.gameObject;
        spawnUI.SetActive(true);       
        spawnUI.transform.Find("Builder").GetComponent<Button>().onClick.AddListener(SpawnBuilder);
        spawnUI.transform.Find("Miner").GetComponent<Button>().onClick.AddListener(SpawnMiner);
        spawnUI.transform.Find("Lumberjack").GetComponent<Button>().onClick.AddListener(SpawnLumberjack);
        
    }
    void SpawnLumberjack()
    {
        if (playerManager.playerResources.Food >= 5)
        {
            playerManager.playerResources.Food -= 5;
            playerManager.UpdateUI();
            GameObject obj = Instantiate(unitPrefabs[0], selectedFortress.transform.position, unitPrefabs[0].transform.rotation);
            obj.transform.SetParent(selectedFortress.transform.parent);
            TurnManagerScript turnManagerScript = FindObjectOfType<TurnManagerScript>();
            turnManagerScript.UnitControls();
            HideFortressUI();
        }
       
    }
    void SpawnMiner()
    {
        if (playerManager.playerResources.Food >= 5)
        {
            playerManager.playerResources.Food -= 5;
            playerManager.UpdateUI();
            GameObject obj = Instantiate(unitPrefabs[1], selectedFortress.transform.position, unitPrefabs[1].transform.rotation);
            obj.transform.SetParent(selectedFortress.transform.parent);
            TurnManagerScript turnManagerScript = FindObjectOfType<TurnManagerScript>();
            turnManagerScript.UnitControls();
            HideFortressUI();
        }
            
    }
    void SpawnBuilder() {
        if(playerManager.playerResources.Food >= 5)
        {
            playerManager.playerResources.Food -= 5;
            playerManager.UpdateUI();
            GameObject obj = Instantiate(unitPrefabs[2], selectedFortress.transform.position, unitPrefabs[2].transform.rotation);
            obj.transform.SetParent(selectedFortress.transform.parent);
            TurnManagerScript turnManagerScript = FindObjectOfType<TurnManagerScript>();
            turnManagerScript.UnitControls();
            HideFortressUI();
        }        
    }
    void Build()
    {
        isBuild = true;
    }
}
