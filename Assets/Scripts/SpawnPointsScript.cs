using UnityEngine;

public class SpawnPointsScript : MonoBehaviour
{
    public static bool isWalls = false;
    GameObject previewWall;
    GameObject previewTower;
    [SerializeField] GameObject newWall;
    [SerializeField] GameObject newTower;
    [SerializeField] GameObject[] wallsSpawnPoints;
    [SerializeField] GameObject[] towersSpawnPoints;

    private void Awake()
    {
        previewWall = GameObject.Find("WallPreview");
        previewTower = GameObject.Find("TowerPreview");

    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 mousePos = hit.point;
            if (isWalls)
            {
                GameObject closest = GetClosesestWallsSpawnPoint(mousePos);

                if (closest != null)
                {
                    previewWall.transform.position = closest.transform.position;
                    previewWall.transform.rotation = closest.transform.rotation;
                    if (Input.GetMouseButtonDown(1))
                    {
                        GameObject wall = Instantiate(newWall, closest.transform.position, closest.transform.rotation);
                        newWall.name = "Wall";
                        Destroy(closest);
                    }
                }
            }
            else
            {
                GameObject closest = GetClosesestTowersSpawnPoint(mousePos);
                if (closest != null)
                {
                    previewTower.transform.position = closest.transform.position;
                    previewTower.transform.rotation = closest.transform.rotation;
                    if (Input.GetMouseButtonDown(1))
                    {
                        GameObject tower = Instantiate(newTower, closest.transform.position, closest.transform.rotation);
                        newTower.name = "Tower";
                        Destroy(closest);
                    }
                }

            }
        }
        else
        {
            return;
        }
    }

    GameObject GetClosesestWallsSpawnPoint(Vector3 mousePos)
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach(GameObject obj in wallsSpawnPoints)
        {
            float dist = Vector3.Distance(mousePos, obj.transform.position);

            if(dist < minDistance)
            {
                minDistance = dist;
                closest = obj;
            }
        }
        return closest;
    }

    GameObject GetClosesestTowersSpawnPoint(Vector3 mousePos)
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject obj in towersSpawnPoints)
        {
            float dist = Vector3.Distance(mousePos, obj.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = obj;
            }
        }
        return closest;
    }
}
