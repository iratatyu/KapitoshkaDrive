using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartGame()
    {
        // Завантаження сцени гри
        SceneManager.LoadScene("Main"); // Замініть "GameScene" на назву вашої сцени
    }

    public void OpenTeamScene()
    {
        // Завантаження сцени з інформацією про команду
        //SceneManager.LoadScene("TeamScene"); // Замініть "TeamScene" на назву вашої сцени
    }

    public void QuitGame()
    {
        // Вихід з гри
        Application.Quit();
    }
}
