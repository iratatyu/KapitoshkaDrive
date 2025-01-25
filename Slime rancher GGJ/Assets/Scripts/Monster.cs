using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 3f; // Ўвидк≥сть руху монстра
    public float attackRange = 1.5f; // ƒистанц≥€ дл€ атаки
    public int attackDamage = 15; // Ўкода в≥д атаки
    public float attackCooldown = 3f; // „ас м≥ж атаками

    private Transform player;
    private bool canAttack = true;

    void Start()
    {
        // «находимо гравц€ за тегом "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene. Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // –ухаЇмос€ до гравц€
            MoveTowardsPlayer();

            // ѕерев≥р€Їмо дистанц≥ю дл€ атаки
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && canAttack)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // –ухаЇмос€ в напр€мку гравц€
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // –озвертаЇмо монстра в напр€мку гравц€
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;

        // ¬иконуЇмо атаку (наприклад, викликаЇмо метод на гравц≥)
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogError("PlayerHealth component not found on the player.");
        }

        // „екаЇмо перед наступною атакою
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }
}
