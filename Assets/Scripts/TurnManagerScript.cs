using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManagerScript : MonoBehaviour
{
    private string[] turn = { "Blue", "Red", "Green", "Purple" };
    private static int turnIndex;

    private void Start()
    {
        turnIndex = 0; //Random.Range(0,turn.Length);
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
        turnIndex++;
        if (turnIndex >= turn.Length)
        {
            turnIndex = 0;
        }
        Debug.Log("Current turn: " + turn[turnIndex]);
        UnitControls();
    }
}
