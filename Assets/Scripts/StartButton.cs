using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void LoadLakeScene()
    {
        Debug.Log("Loading Lake scene...");
        SceneManager.LoadScene("Lake");
    }
}