using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public string bubbleName;
    public float scale = 4;
    public float reproductionChance = 0.1f;
    public GameObject childBubblePrefab;
    public GameObject monsterPrefab; // Prefab for the monster
    public float maturityTime = 300f; // 5 minutes
    public GameObject deathVFX;
    public GameObject movementVFXPrefab; // Prefab for movement VFX
    public float reproductionCooldown = 1800f; // 30 minutes cooldown
    public GameObject dropPrefab;

    public float moveSpeed = 1f; // Speed of movement
    public float moveRadius = 5f; // Radius of random movement

    private Vector3 targetPosition;
    public bool isAdult = true;
    private bool isMature = true;
    private bool canReproduce = true;
    private bool isStopped = false; // Flag to control movement
    public bool IsHeld { get; set; }

    private Rigidbody rb;
    private GameObject movementVFXInstance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"{name} is missing a Rigidbody component!");
        }
        else
        {
            rb.useGravity = true; // Enable gravity for proper grounding
        }

        SetRandomTargetPosition();

        if (scale == 2)
        {
            StartCoroutine(MatureCoroutine());
        }

        StartCoroutine(JumpCoroutine());
    }

    void Update()
    {
        if (!isStopped)
        {
            MoveTowardsTarget();
        }
    }

    public void StopMovement()
    {
        isStopped = true;
        StopAllCoroutines(); // Stop all ongoing coroutines
        rb.isKinematic = true; // Make the Rigidbody kinematic
        rb.linearVelocity = Vector3.zero; // Stop all motion
        rb.angularVelocity = Vector3.zero;
        if (movementVFXInstance != null)
        {
            Destroy(movementVFXInstance); // Stop movement VFX
        }
    }

    public void ResumeMovement()
    {
        isStopped = false;
        rb.isKinematic = false; // Restore Rigidbody movement
        SetRandomTargetPosition(); // Reset target for movement
        StartCoroutine(JumpCoroutine()); // Restart jumping coroutine
        StartMovementVFX(); // Restart movement VFX
    }

    private IEnumerator MatureCoroutine()
    {
        yield return new WaitForSeconds(maturityTime);
        scale = 4;
        isMature = true;
        transform.localScale = Vector3.one * scale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isMature && canReproduce && collision.gameObject.TryGetComponent(out Bubble otherBubble))
        {
            if (bubbleName == otherBubble.bubbleName && otherBubble.isMature && otherBubble.scale == 4 && otherBubble.canReproduce)
            {
                if (Random.value < reproductionChance)
                {
                    Reproduce(otherBubble);
                }
            }
            else if (Random.value < 0.05f) // 5% chance to create a monster
            {
                CreateMonster();
            }
        }
    }

    private void Reproduce(Bubble otherBubble)
    {
        Vector3 spawnPosition = (transform.position + otherBubble.transform.position) / 2;
        GameObject childBubble = Instantiate(childBubblePrefab, spawnPosition, Quaternion.identity);
        childBubble.GetComponent<Bubble>().bubbleName = bubbleName;
        childBubble.GetComponent<Bubble>().scale = 2;
        childBubble.transform.localScale = Vector3.one * 2;

        StartCoroutine(StartReproductionCooldown());
        otherBubble.StartCoroutine(otherBubble.StartReproductionCooldown());
    }

    private void CreateMonster()
    {
        Instantiate(monsterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject); // The bubble turns into a monster
    }

    private IEnumerator StartReproductionCooldown()
    {
        canReproduce = false;
        yield return new WaitForSeconds(reproductionCooldown);
        canReproduce = true;
    }

    private void SetRandomTargetPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection.y = 0; // Keep movement on a horizontal plane
        targetPosition = transform.position + randomDirection;
    }

    private void MoveTowardsTarget()
    {
        if (rb != null && !rb.isKinematic)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }
    }

    private IEnumerator JumpCoroutine()
    {
        while (!isStopped)
        {
            yield return new WaitForSeconds(Random.Range(5f, 15f)); // Random delay between jumps

            Vector3 originalPosition = transform.position;
            Vector3 jumpPosition = originalPosition + new Vector3(0, 4f, 0);

            float jumpTime = 1f; // Time to reach peak
            float landTime = 1f; // Time to land back

            float elapsedTime = 0f;

            // Move upwards
            while (elapsedTime < jumpTime)
            {
                transform.position = Vector3.Lerp(originalPosition, jumpPosition, elapsedTime / jumpTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = jumpPosition;

            elapsedTime = 0f;

            // Move downwards
            while (elapsedTime < landTime)
            {
                transform.position = Vector3.Lerp(jumpPosition, originalPosition, elapsedTime / landTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition;
        }
    }

    private void StartMovementVFX()
    {
        if (movementVFXPrefab != null && movementVFXInstance == null)
        {
            movementVFXInstance = Instantiate(movementVFXPrefab, transform.position, Quaternion.identity);
            movementVFXInstance.transform.SetParent(transform);
        }
    }
    public void Die()
    {
        if (!isAdult) return; // Тільки дорослі бульбашки можуть бути знищені

        Debug.Log("Bubble has died!");

        // Спавнимо предмет
        if (dropPrefab != null)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }

        // Спавнимо VFX
        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }

        // Знищуємо бульбашку
        Destroy(gameObject);
    }
}

