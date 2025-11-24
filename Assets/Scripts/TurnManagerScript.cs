using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TurnManagerScript : MonoBehaviour
{
    private string[] turn = { "Blue", "Red", "Green", "Purple" };
    private PlayerManagerScript player;
    private static int turnIndex;
    public static string currentTurn;
    private bool isFirstRound;

    public void Awake()
    {
        isFirstRound = true;
        player = GameObject.FindAnyObjectByType<PlayerManagerScript>();
        turnIndex = 0; //Random.Range(0,turn.Length);
        currentTurn = turn[turnIndex];
        BuildingControls();

    }
    public void Starting()
    {
        UnitControls();

    }

    public void UnitControls()
    {
        List<GameObject> allUnits = GameObject.FindGameObjectsWithTag("Unit").ToList();
        Debug.Log(allUnits.Count);
        foreach (GameObject unit in allUnits)
        {
            if (unit.layer != LayerMask.NameToLayer(turn[turnIndex]))
            {
                Debug.Log("Disabling unit: " + unit.name);
                unit.GetComponent<UnitsScript>().enabled = false;
            }
            else
            {
                Debug.Log("Enabling unit: " + unit.name);
                unit.GetComponent<UnitsScript>().enabled = true;
            }
        }
    }
    public void BuildingControls()
    {
        List<GameObject> allBuildings = GameObject.FindGameObjectsWithTag("Building").ToList();
        List<GameObject> currentTurnBuildings = new List<GameObject>();
        foreach (GameObject building in allBuildings)
        {
            if (building.layer == LayerMask.NameToLayer(turn[turnIndex]))
            {
                currentTurnBuildings.Add(building);
                Debug.Log(building.name);
            }
        }

        foreach (GameObject fort in currentTurnBuildings)
        {
            if(fort.name == "Fortress")
            {
                fort.GetComponent<PeopleManageScript>().AddPeople(1);
            }
        }
        foreach (GameObject farm in currentTurnBuildings)
        {
            if (farm.name == "Farm")
            {
                player.playerResources.Food += 5;
            }
        }
    }

    public void NextTurn()
    {
        player.SaveResources();
        turnIndex++;
        if (turnIndex >= turn.Length)
        {
            turnIndex = 0;
            isFirstRound = false;

        }
        Debug.Log("Current turn: " + turn[turnIndex]);
        UnitControls();
        
        currentTurn = turn[turnIndex];

        if (isFirstRound)
        {
            player.SetStartResourceValues();
        }
        else
        {
            BuildingControls();
            player.LoadResources();
        }
    }
}
