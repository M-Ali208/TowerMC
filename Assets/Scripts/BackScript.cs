using UnityEngine;
using UnityEngine.SceneManagement;

public class BackScript: MonoBehaviour
{
    public void BackToMainMenuScene()
    {
        SceneManager.LoadScene("StartUI");
    }
}
