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
    private List<Material> listMaterials;
    private List<MeshRenderer> meshRenderers;

    [System.Serializable]
    public class BuildingEntry
    {
        public GameObject building;
        public Button button;
    }
    public List<BuildingEntry> buildingEntries;

    private static GameObject selectedTiles;
    private void OnMouseDown()
    {        
        if (selectedTiles != gameObject)
        {
            if (selectedTiles != null)
            {
                OpenBuildMenu(selectedTiles);
                listMaterials = selectedTiles.GetComponent<MeshRenderer>().materials.ToList();
                listMaterials.RemoveAt(1);
                selectedTiles.GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
                for (int i = 0; i < selectedTiles.transform.childCount; i++)
                {
                    if (selectedTiles.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                        listMaterials = selectedTiles.transform.GetChild(i).GetComponent<MeshRenderer>().materials.ToList();
                    else
                        continue;
                    listMaterials.RemoveAt(1);
                    selectedTiles.transform.GetChild(i).GetComponent<MeshRenderer>().materials = listMaterials.ToArray();
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
            selectedTiles = gameObject;
            
        }
    }

    private void OpenBuildMenu(GameObject selestedTile)
    {
        foreach (var entry in buildingEntries)
        {
            entry.button.onClick.RemoveAllListeners();
            entry.button.onClick.AddListener(() => BuildStructure(entry.building, selestedTile));
        }
    }

    private void BuildStructure(GameObject building, GameObject selectedTile)
    {
        if (selectedTiles != null)
        {            
            Instantiate(building, selectedTile.transform.position, building.transform.rotation);
        }
    }
}

