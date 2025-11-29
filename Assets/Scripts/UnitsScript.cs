using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;



public class UnitsScript : MonoBehaviour
{
    public enum UnitType
    {
        Builder,
        Lumberjack,
        Miner
    }
    UnitType unitType;

    [SerializeField] private List<Button> unitActionsButton;
    [SerializeField] private GameObject buttonBuildPanels;
    private static GameObject currentActiveUnitUI;
    [SerializeField] private GameObject characterCanvas;

    private bool isOnResourceTile = false;
    private List<UnityAction> unityActions = new List<UnityAction>();

    int wood, stone, metal;
    public PlayerManagerScript playerManager;
    private bool isTurn = false;


    [SerializeField] Material mat;
    public int walkPoints = 2;
    public static float walkDistance = GlobalContainer.hexRadius * 2;

    [System.Serializable]
    public class BuildingEntry
    {
        public GameObject building;
        public Button button;
    }
    public List<BuildingEntry> buildingEntries;


    private void Awake()
    {
        playerManager = GameObject.FindAnyObjectByType<PlayerManagerScript>();
        if (gameObject.name == "Builder(Clone)")
        {
            Debug.Log("Builder found");
            unitType = UnitType.Builder;
        }
        else if (gameObject.name == "Lumberjack(Clone)")
        {
            Debug.Log("Lumberjack found");
            unitType = UnitType.Lumberjack;
        }
        else if (gameObject.name == "Miner(Clone)")
        {
            Debug.Log("Miner found");
            unitType = UnitType.Miner;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(AddResources());
        characterCanvas.SetActive(false);      
        walkPoints = 2;
        unitActionsButton.Add(gameObject.transform.Find("Canvas").Find("ActivateButton").GetComponent<Button>());
        foreach(var button in unitActionsButton)
        {
            Debug.Log(button.name);
        }
        if (unitType == UnitType.Builder)
        {
            unityActions = new List<UnityAction>
            {
                UnitMove,
                BuildMenuOpen,
                ShowUnitUI
            };           
        }
        else
        {
            Debug.Log("работает нахуй");
            unityActions = new List<UnityAction>
            {
                UnitMove,
                ShowUnitUI
            };
        }
        for (int i = 0; i < unitActionsButton.Count; i++)
        {
            Debug.Log(i);
            if (i < unityActions.Count)
            {
                int index = i;
                unitActionsButton[i].onClick.AddListener(() => unityActions[index].Invoke());
            }
            else
            {
                Debug.LogError($"Индекс {i} выходит за пределы списка unityActions.");
            }
        }
        
    }
    
    private void OnDisable()
    {
        characterCanvas.SetActive(false);
        for (int i = 0; i < unitActionsButton.Count; i++)
        {
            unitActionsButton[i].onClick.RemoveAllListeners();
        }
    }
    public void UnitMove()
    {
        if(walkPoints > 0)
        {
            GameObject tileWithUnit = gameObject.transform.parent.gameObject;
            Debug.Log(GlobalContainer.trueAllTiles.Count);
            tileWithUnit.GetComponent<ClickLogick>().MultiplyDiselected();
            foreach (var tile in GlobalContainer.trueAllTiles)
            {
                float distance = Vector3.Distance(tile.transform.position, tileWithUnit.transform.position);
                if (distance <= walkDistance && walkPoints >= 2)
                {
                    tile.GetComponent<ClickLogick>().SelectedMultiply(gameObject);
                }
                else if (distance <= walkDistance / 2  && walkPoints == 1)
                {
                    tile.GetComponent<ClickLogick>().SelectedMultiply(gameObject);
                }
            }         
        }
        else
        {
            Debug.Log("No walk points left!");
        }
    }

    public void BuildMenuOpen()
    {
        Debug.Log("BuildMenuOpen");
        buttonBuildPanels.SetActive(true);
        foreach (var entry in buildingEntries)
        {
            entry.button.onClick.RemoveAllListeners();
            entry.button.onClick.AddListener(() =>
            {
                Build(entry.building);
            });
        }
    }
    public void Build(GameObject build) {

        if(gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer(TurnManagerScript.currentTurn) && !gameObject.transform.parent.gameObject.GetComponentInChildren<BuildFunctional>())
        {
            buttonBuildPanels.SetActive(false);
            if (playerManager.playerResources.Wood >= 10 && playerManager.playerResources.Stone >= 10)
            {
                playerManager.playerResources.Wood -= 10;
                playerManager.playerResources.Stone -= 10;
            }
            else
            {
                Debug.Log("Not enough resources to build!");
                return;
            }
            playerManager.UpdateUI();
            GameObject tileWithUnit = gameObject.transform.parent.gameObject;
            GameObject newBuilding = Instantiate(build, tileWithUnit.transform.position, tileWithUnit.transform.rotation);
            newBuilding.transform.parent = tileWithUnit.transform;
            newBuilding.layer = tileWithUnit.layer;
        }
        else
        {
            Debug.Log("It's not your territory!");
        }
        
    }

    public void UnitsScenary()
    {
        if (unitType == UnitType.Miner)
        {
            Debug.Log("Checking for mining options...");
            if (gameObject.transform.parent.Find("Rock(Clone)") && gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer(TurnManagerScript.currentTurn))
            {
                isOnResourceTile = true;
                Debug.Log("Can mine stone here!");
            }
            else
            {
                isOnResourceTile = false;
            }
        }
        else if (unitType == UnitType.Lumberjack)
        {
            Debug.Log("Checking for wood chopping options...");
            if (gameObject.transform.parent.Find("Forest(Clone)") && gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer(TurnManagerScript.currentTurn))
            {
                isOnResourceTile = true;
                Debug.Log("Can chop wood here!");
            }
            else
            {
                isOnResourceTile = false;
            }
        }
    }
    public void ShowUnitUI()
    {
        if (currentActiveUnitUI != null && currentActiveUnitUI != characterCanvas)
        {
            currentActiveUnitUI.SetActive(false);
        }
        characterCanvas.SetActive(!characterCanvas.activeSelf);
        currentActiveUnitUI = characterCanvas;
    }
    private IEnumerator AddResources()
    {
        yield return new WaitForSeconds(0.001f);
        if (isOnResourceTile && unitType == UnitType.Miner)
        {
            playerManager.AddResources(10, "Stone");
        }
        else if (isOnResourceTile && unitType == UnitType.Lumberjack)
        {
            playerManager.AddResources(10, "Wood");
        }
    }

}
