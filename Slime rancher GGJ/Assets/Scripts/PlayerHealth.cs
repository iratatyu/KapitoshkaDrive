using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Максимальне здоров'я
    public int currentHealth;  // Поточне здоров'я
    public TextMeshProUGUI healthText; // UI текст для відображення HP
    public string deathSceneName = "DeathScene"; // Назва сцени смерті
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth; // Встановлюємо початкове здоров'я
        UpdateHealthUI(); // Оновлюємо UI
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Обмежуємо здоров'я

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealToFull()
    {
        if (isDead) return;

        currentHealth = maxHealth; // Відновлюємо здоров'я
        UpdateHealthUI();
        Debug.Log("Health fully restored!");
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player has died!");
        SceneManager.LoadScene(deathSceneName); // Завантажуємо сцену смерті
    }
}
