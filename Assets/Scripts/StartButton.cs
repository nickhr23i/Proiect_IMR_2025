using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public float fadeSpeed = 1f;
    private float alpha = 0f;
    private bool isFading = false;
    private string sceneToLoad;

    public void LoadLakeScene()
    {
        sceneToLoad = "Lake";
        isFading = true;
    }

    void OnGUI()
    {
        if (!isFading) return;

        alpha += Time.deltaTime * fadeSpeed;
        GUI.color = new Color(0, 0, 0, alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);

        if (alpha >= 1f)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
