using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu";

    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
