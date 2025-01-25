using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour
{
    public string bubbleName;
    public float scale = 4;
    public float reproductionChance = 0.1f; // Reduced reproduction chance
    public GameObject childBubblePrefab;
    public float maturityTime = 300f; // 5 minutes
    public GameObject deathReplacementPrefab;
    public GameObject deathVFX;
    public float reproductionCooldown = 1800f; // 30 minutes cooldown

    public float moveSpeed = 1f; // Speed of movement
    public float moveRadius = 5f; // Radius of random movement

    private Vector3 targetPosition;
    private bool isMature = true;
    private bool canReproduce = true;

    private Rigidbody rb;

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
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isMature && other.CompareTag("DeathZone"))
        {
            Die();
        }
    }

    private IEnumerator MatureCoroutine()
    {
        yield return new WaitForSeconds(maturityTime);
        scale = 4;
        isMature = true;
        transform.localScale = Vector3.one * scale;
    }

    private void Die()
    {
        Instantiate(deathVFX, transform.position, Quaternion.identity);
        Instantiate(deathReplacementPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{name} collided with {collision.gameObject.name}");

        if (isMature && canReproduce && collision.gameObject.TryGetComponent(out Bubble otherBubble))
        {
            if (bubbleName == otherBubble.bubbleName && otherBubble.isMature && otherBubble.scale == 4 && otherBubble.canReproduce)
            {
                Debug.Log($"{name} is attempting to reproduce with {otherBubble.name}");
                if (Random.value < reproductionChance)
                {
                    Reproduce(otherBubble);
                }
                else
                {
                    Debug.Log("Reproduction attempt failed due to chance.");
                }
            }
            else
            {
                Debug.Log("Conditions for reproduction not met: " +
                          $"Matching names: {bubbleName == otherBubble.bubbleName}, " +
                          $"Both mature: {isMature && otherBubble.isMature}, " +
                          $"Both scale 4: {scale == 4 && otherBubble.scale == 4}, " +
                          $"Both can reproduce: {canReproduce && otherBubble.canReproduce}");
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

        Debug.Log($"A new child bubble of type {bubbleName} has been created at {spawnPosition}");

        StartCoroutine(StartReproductionCooldown());
        otherBubble.StartCoroutine(otherBubble.StartReproductionCooldown());
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
}