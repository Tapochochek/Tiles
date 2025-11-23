using UnityEngine;

public class BuildFunctional : MonoBehaviour
{
    PlayerManagerScript playerManagerScript;
    private void Awake()
    {
        playerManagerScript = GameObject.Find("PlayerManager").GetComponent<PlayerManagerScript>();
    }
    private void OnEnable()
    {
        playerManagerScript.LoadResources();
        if(gameObject.name == "Farm")
        {
            playerManagerScript.playerResources.Food += 5;
            playerManagerScript.SaveResources();
            playerManagerScript.LoadResources();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
