
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void OpenGameScene()
    {
        SceneManager.LoadScene ("Game");
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene (name);
    }
    public void Exit()
    {
        Application.Quit ();
    }
}
