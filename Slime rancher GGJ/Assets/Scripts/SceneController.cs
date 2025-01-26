using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartGame()
    {
        // ������������ ����� ���
        SceneManager.LoadScene("Main"); // ������ "GameScene" �� ����� ���� �����
    }

    public void OpenTeamScene()
    {
        // ������������ ����� � ����������� ��� �������
        //SceneManager.LoadScene("TeamScene"); // ������ "TeamScene" �� ����� ���� �����
    }

    public void QuitGame()
    {
        // ����� � ���
        Application.Quit();
    }
}
