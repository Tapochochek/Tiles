using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoadScene : MonoBehaviour
{
    public void ButtonClick(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void ButtonExitClick()
    {
        Application.Quit();
    }
}
