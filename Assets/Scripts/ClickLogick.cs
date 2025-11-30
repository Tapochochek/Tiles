using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ClickLogick : MonoBehaviour
{
    public Material mat;
    public Material moveMat;
    private static Material defaultMaterial;
    public GameObject peopleCountPrefab;
    private List<Material> listMaterials;
    private List<MeshRenderer> meshRenderers;
    private static GameObject currenUnit;
    private static GameObject selectedTile;
    private static GameObject currentSpawnPoints;

    [SerializeField] private GameObject spawnPointsBuildings;

    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject wallPreview;

    [SerializeField] GameObject towerPreview;
    [SerializeField] GameObject towerPrefab;


    private static List<GameObject> selectedTiles = new List<GameObject>();
    public static bool isFortressUI = false;

    private void Start()
    {
        defaultMaterial = GetComponent<Renderer>().material;
    }
    private void OnMouseOver()
    {
        RightClickOnZone(gameObject);       
    }
    // Готовый метод выделения тайла
    private void OnMouseDown()
    {
        PeopleManageScript peopleManage = null;
        if (isFortressUI)
        {
            return;
        }
        //Проверка если ни одни тайл не выделен
        if (selectedTile != null)
        {
            if (selectedTile.GetComponentInChildren<PeopleManageScript>())
            {
                peopleManage = selectedTile.GetComponentInChildren<PeopleManageScript>();
            }
            Diselected(selectedTile);            
        }
            
        //Снятие выделения со всех выделенных тайлов
        MultiplyDiselected();
        //Выделение текущего тайла
        PaintingTiles(mat);
               
        selectedTile = gameObject;
        if (selectedTile.GetComponentInChildren<PeopleManageScript>() && selectedTile.layer == LayerMask.NameToLayer(TurnManagerScript.currentTurn))
        {
            StartCoroutine(selectedTile.GetComponentInChildren<PeopleManageScript>().ShowFortressUI());
            if (peopleManage.isBuild) peopleManage.isBuild = false;

            peopleManage.HideFortressUI();
            return;
        }
        if (peopleManage.isBuild && selectedTile.layer == LayerMask.NameToLayer(TurnManagerScript.currentTurn))
        {
            if (currentSpawnPoints != null)
            {
                Destroy(currentSpawnPoints);
                currentSpawnPoints = null;
            }
            GameObject spawnPoints = Instantiate(spawnPointsBuildings, selectedTile.transform.position, selectedTile.transform.rotation, selectedTile.transform);
            currentSpawnPoints = spawnPoints;
            peopleManage.isBuild = false;

            peopleManage.HideFortressUI();
            return;
        }
        
    }
    public void SelectedMultiply(GameObject unit)
    {
        currenUnit = unit;
        if (selectedTile != null)
            Diselected(selectedTile);
        selectedTiles.Add(gameObject);
        PaintingTiles(moveMat);
    }
    public void Diselected(GameObject tile)
    {
        listMaterials = tile.GetComponent<MeshRenderer>().materials.ToList();
        if (!(listMaterials.Count < 2))
            listMaterials.RemoveAt(1);
        tile.GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
        for (int i = 0; i < tile.transform.childCount; i++)
        {
            if (tile.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                listMaterials = tile.transform.GetChild(i).GetComponent<MeshRenderer>().materials.ToList();
            else
                continue;
            if (!(listMaterials.Count < 2))
                listMaterials.RemoveAt(1);
            tile.transform.GetChild(i).GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
        }
    }
    public void MultiplyDiselected()
    {
        if (selectedTiles != null)
        {
            foreach (var obj in selectedTiles)
            {
                obj.GetComponent<ClickLogick>().Diselected(obj);
            }
            selectedTiles.Clear();
        }
    }
    private static void RightClickOnZone(GameObject objects)
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currenUnit.GetComponent<UnitsScript>() && currenUnit.GetComponent<UnitsScript>().walkPoints > 0)
            {
                Debug.Log("RightClick");
                if (selectedTiles != null)
                {
                    float distance = Vector3.Distance(currenUnit.transform.parent.position, objects.transform.position);
                    Debug.Log($"Distance: {distance}");
                    Debug.Log($"WalkPoints before move: {currenUnit.GetComponent<UnitsScript>().walkPoints}");
                    Debug.Log($"WalkDistance: {UnitsScript.walkDistance}");
                    Debug.Log($"WalkPoints: {UnitsScript.walkDistance / 2}");
                    if (distance <= UnitsScript.walkDistance && distance >= UnitsScript.walkDistance / 2)
                    {
                        currenUnit.GetComponent<UnitsScript>().walkPoints -= 2;
                    }
                    else if (distance <= UnitsScript.walkDistance / 2)
                    {
                        currenUnit.GetComponent<UnitsScript>().walkPoints -= 1;
                    }
                    currenUnit.transform.position = objects.transform.position + new Vector3(0, 0.5f, 0);
                    currenUnit.transform.parent = objects.transform;
                    ClickLogick selectedTileScript = objects.GetComponent<ClickLogick>();
                    selectedTileScript.MultiplyDiselected();
                    currenUnit.GetComponent<UnitsScript>().UnitsScenary();
                }
            }
            if (selectedTile != null && selectedTile.GetComponentInChildren<PeopleManageScript>() && selectedTiles.Contains(objects))
            {
                
                Debug.Log(selectedTile.name);

                int countPeople = Convert.ToInt32(selectedTile.transform.Find("Fortress").GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text);

                PeopleManageScript peopleManage = selectedTile.GetComponentInChildren<PeopleManageScript>();
                peopleManage.fortressPeople.People = countPeople;

                ClickLogick selectedTileScript = objects.GetComponent<ClickLogick>();

                if (countPeople > 0)
                {
                    if (objects.layer == LayerMask.NameToLayer(TurnManagerScript.currentTurn))
                    {
                        GameObject peopleCountTextTiles = null;
                        if (!selectedTileScript.GetComponentInChildren<TextMeshProUGUI>())
                        {
                            peopleCountTextTiles = Instantiate(selectedTileScript.peopleCountPrefab, objects.transform);
                            peopleCountTextTiles.GetComponentInChildren<TextMeshProUGUI>().text = countPeople.ToString();
                        }
                        else
                        {
                            peopleCountTextTiles = selectedTileScript.GetComponentInChildren<TextMeshProUGUI>().gameObject;
                            peopleCountTextTiles.GetComponentInChildren<TextMeshProUGUI>().text = (Convert.ToInt32(peopleCountTextTiles.GetComponentInChildren<TextMeshProUGUI>().text) + countPeople).ToString();
                        }                       
                        countPeople = 0;
                    }
                    else if (objects.layer == LayerMask.NameToLayer("Gray"))
                    {
                        objects.GetComponent<Renderer>().material = selectedTile.GetComponent<Renderer>().material;
                        foreach (var item in objects.GetComponentsInChildren<Renderer>())
                        {
                            item.material = selectedTile.GetComponent<Renderer>().material;
                            item.gameObject.layer = LayerMask.NameToLayer(TurnManagerScript.currentTurn);
                        }
                        objects.layer = LayerMask.NameToLayer(TurnManagerScript.currentTurn);
                        countPeople--;
                    }
                    else
                    {
                        objects.GetComponent<Renderer>().material = defaultMaterial;
                        objects.layer = LayerMask.NameToLayer("Gray");
                        countPeople--;
                    }
                    
                    selectedTileScript.MultiplyDiselected();
                                                          
                    peopleManage.SaveResources();
                    selectedTile.transform.Find("Fortress").GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = $"{countPeople}";
                    
                }

            }
        }      
    }

    private void PaintingTiles(Material mat)
    {
        meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
        meshRenderers.Add(gameObject.GetComponent<MeshRenderer>());
        listMaterials = gameObject.GetComponent<MeshRenderer>().materials.ToList();
        if (!listMaterials.Contains(mat))
        {
            listMaterials.Add(mat);
            if (meshRenderers != null)
            {
                foreach (var item in meshRenderers)
                {
                    item.GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
                }
            }
            gameObject.GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
        }
    }
}

