using UnityEngine;

public enum UnitType
{
    None,
    Builder,
    Miner,
    Lumberjack
}
public enum BuildingType
{
    None,
    Fortress,
    Village,
    Farm,
    Wall,
    Tower
}

public class Factory : MonoBehaviour
{
    public static Factory Instance;

    [Header("👱 Префабы юнитов")]
    public GameObject builderPrefab;
    public GameObject minerPrefab;
    public GameObject lumberjackPrefab;

    [Header("🏰 Префабы построек")]
    public GameObject fortressPrefab;
    public GameObject villagePrefab;
    public GameObject wallPrefab;
    public GameObject towerPrefab;
    public GameObject farmPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public GameObject CreateUnit(UnitType type, Vector3 pos)
    {
        GameObject unit;
        switch (type)
        {
            case UnitType.Builder:
                unit = Instantiate(builderPrefab, pos, Quaternion.identity);
                break;
            case UnitType.Miner:
                unit = Instantiate(minerPrefab, pos, Quaternion.identity);
                break;
            case UnitType.Lumberjack:
                unit = Instantiate(lumberjackPrefab, pos, Quaternion.identity);
                break;
            default:
                throw new System.Exception("Unknown unit type");
        }
        return unit;

    }
    public GameObject CreateBuild(BuildingType type, Transform pos, Transform rot)
    {
        GameObject build;
        switch (type)
        {
            case BuildingType.Fortress:
                build = Instantiate(fortressPrefab, pos.position, rot.rotation);
                break;
            case BuildingType.Village:
                build = Instantiate(villagePrefab, pos.position, rot.rotation);
                break;
            case BuildingType.Farm:
                build = Instantiate(farmPrefab, pos.position, rot.rotation);
                break;
            case BuildingType.Wall:
                build = Instantiate(wallPrefab, pos.position, rot.rotation);
                break;
            case BuildingType.Tower:
                build = Instantiate(towerPrefab, pos.position, rot.rotation);
                break;
            default:
                throw new System.Exception("UnknownBuildingType");
        }
        return build;
    }
}
