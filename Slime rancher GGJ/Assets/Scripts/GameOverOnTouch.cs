using UnityEngine;
using UnityEngine.SceneManagement; // Дозволяє перезавантажувати сцену або завершувати гру

public class GameOverOnTouch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Перевірка, чи гравець торкається води
        if (other.CompareTag("Water"))
        {
            Debug.Log("Game Over!"); // Виводить повідомлення в консоль
            EndGame(); // Викликає функцію закінчення гри
        }
    }

    private void EndGame()
    {
        // Наприклад, перезапустити сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Або завершити гру (тільки в побудованій грі, не в редакторі)
        // Application.Quit();
    }
}
