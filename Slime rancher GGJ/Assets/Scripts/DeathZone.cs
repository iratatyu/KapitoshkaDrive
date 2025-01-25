using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Перевіряємо, чи об'єкт має компонент Bubble
        Bubble bubble = other.GetComponent<Bubble>();
        if (bubble != null)
        {
            bubble.Die(); // Викликаємо метод Die
        }
    }
}
