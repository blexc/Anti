using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    float startLivingTime = 7f;
    float livingTime;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        livingTime = startLivingTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        livingTime -= Time.deltaTime;
        Color c = spriteRenderer.material.color;
        c.a = livingTime / startLivingTime;
        spriteRenderer.material.color = c;

        if (c.a <= 0)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null && controller.health < controller.maxHealth)
        {
            FindObjectOfType<AudioManager>().Play("Collectible");
            controller.ChangeHealth(1);
            Destroy(gameObject);
        }
    }
}
