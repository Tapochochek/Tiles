using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
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
    public static GameObject selectedFortress;
    private string fortressId;
    TextMeshProUGUI peopleCountText;

    private void Awake()
    {
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
        ClickLogick.isFortressUI = false;
        Canvas canvas = GameObject.Find("FortressUI").GetComponentInChildren<Canvas>();
        canvas.enabled = false;
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
}
