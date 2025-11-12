using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClickLogick : MonoBehaviour
{
    private string[] turn = { "blue", "red", "green", "purple" };
    private static int turnIndex = 0;

    public Material mat;
    public Material moveMat;
    private List<Material> listMaterials;
    private List<MeshRenderer> meshRenderers;
    private static GameObject currenUnit;

    [System.Serializable]
    public class BuildingEntry
    {
        public GameObject building;
        public Button button;
    }
    public List<BuildingEntry> buildingEntries;

    private static GameObject selectedTile;
    private static List<GameObject> selectedTiles = new List<GameObject>();

    private void OnMouseOver()
    {
        RightClickOnZone(gameObject);
    }
    private void OnMouseDown()
    {
        if (selectedTiles != null)
        {
            foreach (var obj in selectedTiles)
            {
                obj.GetComponent<ClickLogick>().Diselected();
                
            }
            selectedTiles.Clear();
        }
        if (selectedTile != gameObject)
        {              
            if (selectedTile != null)
            {
                //OpenBuildMenu(selectedTiles);
                listMaterials = selectedTile.GetComponent<MeshRenderer>().materials.ToList();
                if (listMaterials.Count > 1)
                    listMaterials.RemoveAt(1);
                selectedTile.GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
                for (int i = 0; i < selectedTile.transform.childCount; i++)
                {
                    if (selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                        listMaterials = selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>().materials.ToList();
                    else
                        continue;
                    listMaterials.RemoveAt(1);
                    selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
                }
            }
            
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
            selectedTile = gameObject;
            
        }
    }
    public void SelectedMultiply(GameObject unit)
    {
        currenUnit = unit;
        selectedTiles.Add(gameObject);
        listMaterials = gameObject.GetComponent<MeshRenderer>().materials.ToList();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                listMaterials = gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().materials.ToList();
            else
                continue;
            gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
        }
        meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
        meshRenderers.Add(gameObject.GetComponent<MeshRenderer>());
        listMaterials = gameObject.GetComponent<MeshRenderer>().materials.ToList();
        if (!listMaterials.Contains(moveMat))
        {
            listMaterials.Add(moveMat);
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
    public void Diselected()
    {
        if (selectedTile == null)
        {
            listMaterials = gameObject.GetComponent<MeshRenderer>().materials.ToList();
            listMaterials.RemoveAt(1);
            gameObject.GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                    listMaterials = gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().materials.ToList();
                else
                    continue;
                listMaterials.RemoveAt(1);
                gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
            }
        }
        else
        {
            listMaterials = selectedTile.GetComponent<MeshRenderer>().materials.ToList();
            if (listMaterials.Count > 1)
                listMaterials.RemoveAt(1);
            selectedTile.GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
            for (int i = 0; i < selectedTile.transform.childCount; i++)
            {
                if (selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                    listMaterials =     selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>().materials.ToList();
                else
                    continue;
                listMaterials.RemoveAt(1);
                selectedTile.transform.GetChild(i).GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
            }
        }
        
    }

    //private void OpenBuildMenu(GameObject selestedTile)
    //{
    //    foreach (var entry in buildingEntries)
    //    {
    //        entry.button.onClick.RemoveAllListeners();
    //        entry.button.onClick.AddListener(() => BuildStructure(entry.building, selestedTile));
    //    }
    //}

    private void BuildStructure(GameObject building, GameObject selectedTile)
    {
        if (selectedTiles != null)
        {            
            Instantiate(building, selectedTile.transform.position, building.transform.rotation);
        }
    }
    private static void RightClickOnZone(GameObject objects)
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("RightClick");
            if (selectedTiles != null && selectedTiles.Contains(objects))
            {
                currenUnit.transform.position = objects.transform.position + new Vector3(0, 0.5f, 0);
                currenUnit.transform.parent = objects.transform;
            }
        }
    }
}

