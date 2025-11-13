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


    [SerializeField] Material mat;

    private float walkDistance = GlobalContainer.hexRadius * 2;

    [System.Serializable]
    public class BuildingEntry
    {
        public GameObject building;
        public Button button;
    }
    public List<BuildingEntry> buildingEntries;

    void Start()
    {
        unityActions = new List<UnityAction>
        {
            UnitMove,
            BuildMenuOpen
        };

        for (int i = 0; i < unitActionsButton.Count; i++)
        {
            Debug.Log(i);
            if (i < unityActions.Count) // ѕроверка, чтобы избежать выхода за пределы массива    
            {
                int index = i;
                unitActionsButton[i].onClick.AddListener(() => unityActions[index].Invoke());
            }
            else
            {
                Debug.LogError($"»ндекс {i} выходит за пределы списка unityActions.");
            }
        }
    }
    public void UnitMove()
    {
        GameObject tileWithUnit = gameObject.transform.parent.gameObject;
        Debug.Log(GlobalContainer.trueAllTiles.Count);
        tileWithUnit.GetComponent<ClickLogick>().MultiplyDiselected();
        foreach (var tile in GlobalContainer.trueAllTiles)
        {
            float distance = Vector3.Distance(tile.transform.position, tileWithUnit.transform.position);
            if (distance <= walkDistance)
            {
                tile.GetComponent<ClickLogick>().SelectedMultiply(gameObject);
            }
        }
    }

    public void BuildMenuOpen()
    {
        Debug.Log("BuildMenuOpen");
        buttonBuildPanels.SetActive(true);
        foreach (var entry in buildingEntries)
        {
            Debug.Log("ffff");
            entry.button.onClick.AddListener(() =>
            {
                Build(entry.building);
            });
        }
    }
    public void Build(GameObject build) {

        playerManager.playerResources.Wood -= 10;
        Debug.Log("Wood after build: " + playerManager.playerResources.Wood);
        playerManager.playerResources.Metal -= 10;
        playerManager.playerResources.Stone -= 10;

        playerManager.SaveResources();
        playerManager.LoadResources();
        GameObject tileWithUnit = gameObject.transform.parent.gameObject;
        GameObject newBuilding = Instantiate(build, tileWithUnit.transform.position, tileWithUnit.transform.rotation);
        newBuilding.transform.parent = tileWithUnit.transform;
        buttonBuildPanels.SetActive(false);
    }
}
