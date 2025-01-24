using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera playerCamera;
    public float grabRange = 5f;
    public KeyCode grabKey = KeyCode.E; // E для захоплення
    public KeyCode fireKey = KeyCode.Mouse0; // ЛКМ для пострілу
    public float fireForce = 10f;
    private GameObject heldObject = null;
    private Rigidbody heldObjectRb = null;
    private Transform holdPoint;

    void Start()
    {
        // Створимо точку, де буде утримуватись об'єкт
        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.SetParent(playerCamera.transform);
        holdPoint.localPosition = new Vector3(0, 0, 2); // Встановлюємо точку перед гравцем
    }

    void Update()
    {
        // Пошук об'єктів для захоплення
        if (heldObject == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grabRange))
            {
                if (hit.collider.CompareTag("Pickup"))
                {
                    if (Input.GetKeyDown(grabKey))
                    {
                        // Захоплюємо об'єкт
                        heldObject = hit.collider.gameObject;
                        heldObjectRb = heldObject.GetComponent<Rigidbody>();
                        heldObjectRb.isKinematic = true; // Зупиняємо фізику, поки утримуємо
                        heldObject.SetActive(false); // Приховуємо об'єкт зі сцени
                        heldObject.transform.position = holdPoint.position;
                        heldObject.transform.SetParent(holdPoint);
                    }
                }
            }
        }

        // Стрільба об'єктами
        if (heldObject != null && Input.GetKeyDown(fireKey))
        {
            // Викидаємо об'єкт у напрямку камери
            heldObject.transform.SetParent(null);
            heldObject.SetActive(true); // Показуємо об'єкт знову на сцені
            heldObjectRb.isKinematic = false;

            // Додаємо фізичний рух об'єкта в напрямку камери
            Vector3 fireDirection = playerCamera.transform.forward;
            heldObjectRb.AddForce(fireDirection * fireForce, ForceMode.Impulse);

            heldObject = null; // Після викиду скидаємо об'єкт
        }
    }
}
