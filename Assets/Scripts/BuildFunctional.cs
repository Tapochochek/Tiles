using UnityEngine;

public class BuildFunctional : MonoBehaviour
{
    PlayerManagerScript playerManagerScript;
    private void Awake()
    {
        playerManagerScript = GameObject.Find("PlayerManager").GetComponent<PlayerManagerScript>();
    }
    public void Farm()
    {
        if(gameObject.name == "Farm(Clone)")
        {
            playerManagerScript.playerResources.Food += 5;
            playerManagerScript.UpdateUI();
        }
            

    }

    public void Fort()
    {
        if (gameObject.name == "Fortress")
            gameObject.GetComponentInParent<PeopleManageScript>().AddPeople(1);
    }

    public void Village()
    {
        if(gameObject.name == "Village(Clone)")
        {
            PeopleManageScript peopleManageScript;
            PeopleManageScript[] allFort = FindObjectsByType<PeopleManageScript>(FindObjectsSortMode.None);
            for (int i = 0; i < allFort.Length; i++)
            {
                if (allFort[i].gameObject.layer == LayerMask.NameToLayer(TurnManagerScript.currentTurn))
                {
                    Debug.Log("Добавили типочка");
                    peopleManageScript = allFort[i];
                    peopleManageScript.AddPeople(1);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
