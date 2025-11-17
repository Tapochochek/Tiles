using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManagerScript : MonoBehaviour
{
    private string[] turn = { "Blue", "Red", "Green", "Purple" };
    private PlayerManagerScript player;
    private static int turnIndex;
    public static string currentTurn;
    private bool isFirstRound;

    private void Start()
    {
        isFirstRound = true;
        player = GameObject.FindAnyObjectByType<PlayerManagerScript>();
        turnIndex = 0; //Random.Range(0,turn.Length);
        currentTurn = turn[turnIndex];
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
                unit.GetComponent<UnitsScript>().enabled = false;
            }
            else
            {
                unit.GetComponent<UnitsScript>().enabled = true;
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
            player.LoadResources();
        }
    }
}
