using UnityEngine;

public class Cage : MonoBehaviour
{
    public Transform cageCenter; // Центр клітки, куди телепортуються бульбашки
    public LayerMask bubbleLayer; // Шар бульбашок
    public LayerMask playerLayer; // Шар гравця

    private void OnTriggerEnter(Collider other)
    {
        // Перевіряємо, чи це бульбашка
        if (((1 << other.gameObject.layer) & bubbleLayer) != 0)
        {
            Bubble bubble = other.GetComponent<Bubble>();

            // Якщо бульбашка не несе гравець, телепортуємо її в центр клітки
            if (bubble != null && !bubble.IsHeld)
            {
                Debug.Log("Бульбашка намагається ввійти в клітку самостійно!");
                TeleportToCageCenter(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Перевіряємо, чи це бульбашка
        if (((1 << other.gameObject.layer) & bubbleLayer) != 0)
        {
            Bubble bubble = other.GetComponent<Bubble>();

            // Якщо бульбашка несе гравець, дозволяємо їй вийти
            if (bubble != null && bubble.IsHeld)
            {
                Debug.Log("Гравець виніс бульбашку з клітки.");
            }
            else
            {
                // Якщо бульбашка виходить сама, не дозволяємо
                Debug.Log("Бульбашка намагається вийти з клітки самостійно!");
                TeleportToCageCenter(other.transform);
            }
        }
    }

    private void TeleportToCageCenter(Transform bubbleTransform)
    {
        // Телепортуємо бульбашку в центр клітки
        bubbleTransform.position = cageCenter.position;
        bubbleTransform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // Зупиняємо рух
    }
}
