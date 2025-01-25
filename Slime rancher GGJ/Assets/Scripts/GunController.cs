using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera playerCamera;
    public float grabRange = 5f;
    public KeyCode grabKey = KeyCode.E;
    public KeyCode fireKey = KeyCode.Mouse0;
    public float fireForce = 10f;

    private Bubble heldBubble = null; // ����� �� �������� ��������� �� Bubble
    private Transform holdPoint;

    void Start()
    {
        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.SetParent(playerCamera.transform);
        holdPoint.localPosition = new Vector3(0, 0, 2);
    }

    void Update()
    {
        if (heldBubble == null)
        {
            TryGrabBubble();
        }
        else
        {
            if (Input.GetKeyDown(fireKey))
            {
                ThrowBubble();
            }
        }
    }

    private void TryGrabBubble()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grabRange))
        {
            if (hit.collider.CompareTag("Bubble"))
            {
                if (Input.GetKeyDown(grabKey))
                {
                    heldBubble = hit.collider.GetComponent<Bubble>();
                    if (heldBubble != null)
                    {
                        heldBubble.StopMovement(); // Зупиняємо рух бульбашки
                        heldBubble.transform.SetParent(holdPoint);
                        heldBubble.transform.localPosition = Vector3.zero;
                    }
                }
            }
        }
    }

    private void ThrowBubble()
    {
        if (heldBubble != null)
        {
            heldBubble.transform.SetParent(null);
            heldBubble.ResumeMovement(); // Відновлюємо рух бульбашки

            Rigidbody rb = heldBubble.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(playerCamera.transform.forward * fireForce, ForceMode.Impulse); // Додаємо імпульс
            }

            heldBubble = null;
        }
    }


}
