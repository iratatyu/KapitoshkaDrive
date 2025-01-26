using UnityEngine;
using UnityEngine.SceneManagement; // �������� ����������������� ����� ��� ����������� ���

public class GameOverOnTouch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // ��������, �� ������� ��������� ����
        if (other.CompareTag("Water"))
        {
            Debug.Log("Game Over!"); // �������� ����������� � �������
            EndGame(); // ������� ������� ��������� ���
        }
    }

    private void EndGame()
    {
        // ���������, ������������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // ��� ��������� ��� (����� � ���������� ��, �� � ��������)
        // Application.Quit();
    }
}
