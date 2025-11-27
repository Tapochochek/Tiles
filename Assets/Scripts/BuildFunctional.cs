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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
