using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    void Start()
    {
        if (GlobalContainer.firstStep)
        {
            GlobalContainer.allTiles.Add(gameObject);
            GlobalContainer.firstStep = false;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GlobalContainer.availablePlaces.Add(gameObject.transform.GetChild(i).gameObject);
            }
        }
        if(GlobalContainer.MaxMapSize > 0 && !GlobalContainer.firstStep)
        {
            int rand = Random.Range(0, GlobalContainer.availablePlaces.Count);
            GameObject tile = Instantiate(prefab, GlobalContainer.availablePlaces[rand].transform.position, GlobalContainer.availablePlaces[rand].transform.rotation);
            tile.name = "Tile " + (GlobalContainer.allTiles.Count + 1);
            GlobalContainer.allTiles.Add(tile);
            for (int i = 0; i < tile.transform.childCount-1; i++)
            {
                GlobalContainer.availablePlaces.Add(tile.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < GlobalContainer.allTiles.Count; i++)
            {
                for (int j = GlobalContainer.availablePlaces.Count - 1; j >= 0; j--)
                {
                    if (GlobalContainer.allTiles[i].GetComponent<Collider>().bounds.Intersects(GlobalContainer.availablePlaces[j].GetComponent<Collider>().bounds))
                    {
                        GlobalContainer.availablePlaces.RemoveAt(j);
                    }
                }
            }

        }
        else if(GlobalContainer.MaxMapSize == 0)
        {
            Debug.Log("Map is created");
        }
        GlobalContainer.MaxMapSize--;
    }
}
