using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class UnitsScript : MonoBehaviour
{
    [SerializeField] private List<Button> unitActionsButton;
    [SerializeField] private GameObject buttonBuildPanels;
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
    private void OnEnable()
    {
        walkPoints = 2; 
        unityActions = new List<UnityAction>
        {
            UnitMove,
            BuildMenuOpen
        };
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

        buttonBuildPanels.SetActive(false);
        if (playerManager.playerResources.Wood >= 10 && playerManager.playerResources.Metal >= 10 && playerManager.playerResources.Stone >= 10)
        {
            playerManager.playerResources.Wood -= 10;
            playerManager.playerResources.Metal -= 10;
            playerManager.playerResources.Stone -= 10;
        }
        else
        {

            Debug.Log("Not enough resources to build!");
            return;
        }
        playerManager.SaveResources();
        playerManager.LoadResources();
        GameObject tileWithUnit = gameObject.transform.parent.gameObject;
        GameObject newBuilding = Instantiate(build, tileWithUnit.transform.position, tileWithUnit.transform.rotation);
        newBuilding.transform.parent = tileWithUnit.transform;
        
    } 
    public void PointMovment()
    {
        GameObject tileWithUnit = gameObject.transform.parent.gameObject;
    }
}
