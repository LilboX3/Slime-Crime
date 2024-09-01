using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    private float speed = 1.1f;
    private int damage = 1;
    private float followRange = 10f;
    private float rotationSpeed = 5f;
    private float lifeCycleSeconds = 15f;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        StartCoroutine(KillAfterSeconds(lifeCycleSeconds));
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= followRange)
            {
                animator.SetTrigger("playernear");
                Vector3 direction = (player.position - transform.position).normalized;

                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            }
        }
    }

    IEnumerator KillAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("playercollision");
            collision.gameObject.GetComponent<SlimeController>().TakeDamage(damage);

            // Optional: Gegner könnte nach dem Aufprall gestoppt, zerstört oder zurückgesetzt werden
            // Destroy(gameObject); // Falls der Gegner nach der Kollision verschwinden soll
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ocean"))
        {
            Destroy(gameObject);
        }
    }
}
