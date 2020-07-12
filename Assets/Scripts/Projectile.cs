using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    float startDeleteTimer = 10f;
    float deleteTimer;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        deleteTimer = startDeleteTimer;
    }

    private void Update()
    {
        deleteTimer -= Time.deltaTime;
        if (deleteTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(1, 0);
            Destroy(gameObject);
        }
    }
}
