using UnityEngine;
using System.Collections.Generic;

public class Cage : MonoBehaviour
{
    public string playerTag = "Player"; // Тег для гравця
    public string bubbleTag = "Bubble"; // Тег для бульбашок

    // Список бульбашок, які заніс гравець
    private HashSet<GameObject> carriedBubbles = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // Якщо гравець заходить у клітку
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player entered the cage.");
            return;
        }

        // Якщо бульбашка заходить у клітку
        if (other.CompareTag(bubbleTag))
        {
            if (carriedBubbles.Contains(other.gameObject))
            {
                Debug.Log($"Bubble {other.name} entered the cage (carried by player).");
                carriedBubbles.Remove(other.gameObject); // Видаляємо з переліку
            }
            else
            {
                Debug.Log($"Bubble {other.name} entered the cage by itself.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Якщо гравець виходить із клітки
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player left the cage.");
            return;
        }

        // Якщо бульбашка намагається вийти з клітки
        if (other.CompareTag(bubbleTag))
        {
            if (!carriedBubbles.Contains(other.gameObject))
            {
                Debug.Log($"Bubble {other.name} is not allowed to leave the cage!");
                // Зупиняємо рух і повертаємо бульбашку назад
                Rigidbody bubbleRb = other.GetComponent<Rigidbody>();
                if (bubbleRb != null)
                {
                    bubbleRb.linearVelocity = Vector3.zero;
                    bubbleRb.angularVelocity = Vector3.zero;
                }

                // Переміщуємо бульбашку назад у клітку
                Vector3 cageCenter = transform.position;
                other.transform.position = new Vector3(cageCenter.x, other.transform.position.y, cageCenter.z);
            }
            else
            {
                Debug.Log($"Bubble {other.name} left the cage (carried by player).");
                carriedBubbles.Remove(other.gameObject); // Видаляємо з переліку
            }
        }
    }

    // Метод для додавання бульбашки, яку заніс гравець
    public void AddCarriedBubble(GameObject bubble)
    {
        if (!carriedBubbles.Contains(bubble))
        {
            carriedBubbles.Add(bubble);
            Debug.Log($"Bubble {bubble.name} marked as carried by the player.");
        }
    }

    // Метод для видалення бульбашки, якщо потрібно
    public void RemoveCarriedBubble(GameObject bubble)
    {
        if (carriedBubbles.Contains(bubble))
        {
            carriedBubbles.Remove(bubble);
            Debug.Log($"Bubble {bubble.name} removed from carried list.");
        }
    }
}
