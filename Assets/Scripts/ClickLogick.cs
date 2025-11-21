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
    public GameObject peopleCountPrefab;
    private List<Material> listMaterials;
    private List<MeshRenderer> meshRenderers;
    private static GameObject currenUnit;
    private static GameObject selectedTile;
    private static List<GameObject> selectedTiles = new List<GameObject>();
    public static bool isFortressUI = false;

    private void OnMouseOver()
    {
        RightClickOnZone(gameObject);       
    }
    // Готовый метод выделения тайла
    private void OnMouseDown()
    {
        if (isFortressUI)
        {
            return;
        }
        //Проверка если ни одни тайл не выделен
        if (selectedTile != null)
        {
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
                        GameObject peopleCountTextTiles = Instantiate(selectedTileScript.peopleCountPrefab, objects.transform);
                        peopleCountTextTiles.GetComponentInChildren<TextMeshProUGUI>().text = countPeople.ToString();
                        countPeople = 0;
                    }
                    else
                    {
                        objects.GetComponent<Renderer>().material = selectedTile.GetComponent<Renderer>().material;
                        objects.layer = LayerMask.NameToLayer(TurnManagerScript.currentTurn);
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

