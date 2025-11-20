using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// изначально класс просто хранил информацию о глобальных переменных, сейчас же в нем реализована логика
/// финальной части генерации карты, с расстановкой ресурсов и всего на карте. Логика постройки карты в MapCreator
/// </summary>
public class GlobalContainer : MonoBehaviour
{
    //Внутринние переменные для работы скрипта
    public static int MaxMapSize = 300;   
    public static bool firstStep = true;
    private bool completed = false;
    public static float hexRadius = 1.8f;
    private float radiusSpawn;

    //все тайлы надо для правильной генерации карты, по ходу генерации удаляются доступные, тоетсь на которых чето уже стоит
    public static List<GameObject> allTiles = new List<GameObject>();

    public static List<GameObject> trueAllTiles = new List<GameObject>();

    //доступные места для спавна новых тайлов
    public static List<GameObject> availablePlaces = new List<GameObject>();

    //Лист для тайлов которые нужно запретить для спауна
    private List<GameObject> bannedTiles = new List<GameObject>();

    public GameObject fortrest;

    [SerializeField] private GameObject StartGameManager;

    //Массив для ресурсов (на память 0 - камень, 1 - дерево)
    [SerializeField] private GameObject[] resources;
    

    

    [SerializeField] List<Material> materials;
    public static List<Material> usedMaterials;

    /// <summary>
    /// Инициализация переменных
    /// </summary>
    private void Start()
    {
        usedMaterials = materials;
        radiusSpawn = hexRadius * 2;
    }

    private void Update()
    {
        
        if (MaxMapSize<=0 && !completed)
        {
            int i = 0;
            while(i< materials.Count)
            {
                int rand = Random.Range(0, allTiles.Count);
                GameObject tile = allTiles[rand];
                if (!tile.CompareTag("no"))
                {
                    tile.GetComponent<Renderer>().material = materials[i];
                    GameObject fort = Instantiate(fortrest, tile.transform.position, fortrest.transform.rotation);
                    fort.GetComponent<Renderer>().material = materials[i];
                    fort.layer = LayerMask.NameToLayer(materials[i].name);
                    fort.tag = "Building";
                    fort.name = "Fortress";

                    foreach (var singletile in allTiles)
                    {                         
                        float distance = Vector3.Distance(tile.transform.position, singletile.transform.position);
                        
                        if (distance <= radiusSpawn)
                        {
                            singletile.tag = "no";
                            tile.name = materials[i].name;
                            
                            tile.layer = LayerMask.NameToLayer(materials[i].name);
                            tile.tag = "yes";
                        }
                        if (distance <= hexRadius)
                        {                           
                            singletile.GetComponent<Renderer>().material = materials[i];
                            singletile.layer = LayerMask.NameToLayer(materials[i].name);
                            bannedTiles.Add(singletile);
                        }
                    }
                    fort.transform.SetParent(tile.transform);
                    allTiles.Remove(tile);
                    i++;
                }
                else
                {
                    Debug.Log($"Место не крутое {tile.name}, цвет {materials[i].name}");
                }
            }
            //Перебираем забаненные тайлы и удаляем их из всех тайлов
            foreach (var bannedTile in bannedTiles)
            {
                allTiles.Remove(bannedTile);
            }
            bannedTiles.Clear();

            //Пока что для тестов по двадцадке каждого это потом балансить надо когда пойму на сколько ресы ценные, надо делать их расходными
            ResourcesSpawn(20, 20);
                       
            Debug.Log(allTiles.Count);
            StartGameManager.SetActive(true);
            completed = true;
            
        }
        foreach (var tile in trueAllTiles)
        {
            tile.tag = "Tile";
        }

    }

    /// <summary>
    /// Метод для спауна ресурсов на карте пока что только деревья и камни, потенциально паучьи пещеры и еще че нить, как то надо сделать деньги
    /// </summary>
    /// <param name="rockCount">Количество камней(логично)</param>
    /// <param name="treesCount">Количество деревьев(тоже логично)</param>
    private void ResourcesSpawn(int rockCount, int treesCount)
    {
        //Спавн камнев
        for (int i = 0; i < rockCount; i++)
        {
            int rand = Random.Range(0, allTiles.Count);
            GameObject tile = allTiles[rand];
            allTiles.Remove(tile);
            GameObject rockInst = Instantiate(resources[0], tile.transform.position, resources[0].transform.rotation);
            rockInst.transform.SetParent(tile.transform);
        }
        //Спавн деревьев
        for(int i = 0; i < treesCount; i++)
        {
            int rand = Random.Range(0, allTiles.Count);
            GameObject tile = allTiles[rand];
            allTiles.Remove(tile);
            GameObject treeInst = Instantiate(resources[1], tile.transform.position, resources[1].transform.rotation);
            treeInst.transform.SetParent(tile.transform);
        }
    }
}



