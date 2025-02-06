using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void SettingsButton()
    {
        SceneManager.LoadScene(2);
    }
    public void CreditsButton()
    {
        SceneManager.LoadScene(3);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

}
