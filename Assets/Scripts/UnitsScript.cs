using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UnitsScript : MonoBehaviour
{
    [SerializeField] private List<Button> unitActionsButton;
    private List<UnityAction> unityActions = new List<UnityAction>();
    [SerializeField] Material mat;

    private float walkDistance = GlobalContainer.hexRadius * 2;

    void Start()
    {
        unityActions = new List<UnityAction>
       {
           UnitMove,
           Build
       };

        for (int i = 0; i < unitActionsButton.Count; i++)
        {
            Debug.Log(i);
            if (i < unityActions.Count) // Проверка, чтобы избежать выхода за пределы массива    
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
        //ClickLogick[] clickLogicks = FindObjectsOfType<ClickLogick>();
        //if (clickLogicks.Length > 0)
        //{
        //    foreach (var clickLogick in clickLogicks)
        //    {
        //        clickLogick.Diselected(clickLogick.gameObject);
        //    }
        //}
        //else
        //{
        //    Debug.LogError("ClickLogick объекты не найдены в сцене.");
        //}
    }

    public void Build()
    {
        Debug.Log("Build");
    }
}
