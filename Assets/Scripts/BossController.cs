using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossController : MonoBehaviour
{
    public int state { get; private set; }

    public int maxHealth = 15;
    public GameObject bossBody;
    public GameObject projectilePrefab;
    public GameObject player;
    public EnemyController[] enemies;
    public PowerBlockController[] powerBlockControllers;

    int health;
    int damageThisState;

    public float startRainbowTimer = 4;
    float rainbowTimer;
    public float startShootTimer = 3;
    float shootTimer;

    Animator animator;
    SpriteRenderer[] spriteRenderer = new SpriteRenderer[2];

    void Start()
    {
        state = 0;
        animator = GetComponent<Animator>();
        spriteRenderer[0] = GetComponent<SpriteRenderer>();
        spriteRenderer[1] = bossBody.GetComponent<SpriteRenderer>();

        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void Update()
    {
        animator.SetInteger("State", state);
        switch (state)
        {
            // inactive/ awaken
            case 0:
                break;
            // spawn state
            case 1:
                if (rainbowTimer <= 0)
                {
                    int randBlock = Random.Range(0, 4);
                    powerBlockControllers[randBlock].Rainbow();
                    rainbowTimer = startRainbowTimer;
                }
                else
                {
                    rainbowTimer = Mathf.Clamp(rainbowTimer - Time.deltaTime, 0, startRainbowTimer);
                }

                bool allEnemiesDead = true;
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].Health > 0)
                    {
                        allEnemiesDead = false;
                        break;
                    }
                }
                if (allEnemiesDead) ShootState();
                break;
            // shooting state
            case 2:
                if (shootTimer <= 1)
                {
                    animator.SetTrigger("Grow");
                }

                if (shootTimer <= 0)
                {
                    // shoot either left middle or right
                    int randDir = Random.Range(0, 3);
                    Launch(randDir);
                    shootTimer = startShootTimer;
                }
                else
                {
                    shootTimer = Mathf.Clamp(shootTimer - Time.deltaTime, 0, startShootTimer);
                }

                if (damageThisState >= maxHealth / 3) SpawnState();
                break;
            case 3:
                animator.SetTrigger("Hit");
                for (int i=0; i < spriteRenderer.Length; i++)
                {
                    Color c = spriteRenderer[i].material.color;
                    c.a -= (Time.deltaTime/5);
                    spriteRenderer[i].material.color = c;
                }

                if (spriteRenderer[0].material.color.a <= 0)
                {
                    player.GetComponent<PlayerController>().TurnToStone();
                    gameObject.SetActive(false);
                }
                break;
            default:
                Debug.LogWarning("state number: " + state + " out of range");
                break;
        }
    }

    public void AwakenState()
    {
        state = 0;
        health = maxHealth;
        FindObjectOfType<PersistantAudioManager>().Play("Climate");
        gameObject.layer = LayerMask.NameToLayer("Default");
        for (int i = 0; i < powerBlockControllers.Length; i++)
        {
            powerBlockControllers[i].Rainbow();
        }
        animator.SetTrigger("Open");
        animator.SetTrigger("Grow");
        bossBody.GetComponent<FloatingEffect>().enabled = true;
        SpawnState();
    }

    public void SpawnState()
    {
        state = 1;
        gameObject.layer = LayerMask.NameToLayer("Default");
        for (int i=0; i < enemies.Length; i++)
        {
            enemies[i].Respawn();
        }

        rainbowTimer = startRainbowTimer;
    }

    public void ShootState()
    {
        state = 2;
        gameObject.layer = LayerMask.NameToLayer("Boss");
        shootTimer = startShootTimer;
        damageThisState = 0;
    }

    public void DieState()
    {
        state = 3; 
        GetComponent<FloatingEffect>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        FindObjectOfType<PersistantAudioManager>().Play("Anti");
    }

    public void TakeDamage(int amount)
    {
        if (state == 2)
        {
            animator.SetTrigger("Hit");
            health -= amount;
            damageThisState++;
        }
        if (health <= maxHealth - 10) GetComponent<FloatingEffect>().enabled = true;
        if (health <= 0) DieState();
    }

    void Launch(int direction)
    {
        float launchForce = 500;

        GameObject projectileObject = Instantiate(projectilePrefab,
            transform.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        Vector2 playerPos = player.GetComponent<Rigidbody2D>().position;

        switch (direction)
        {
            case 0: // left
                projectile.Launch(new Vector2(-0.5f, -1), launchForce);
                break;
            case 1: // middle
                projectile.Launch(Vector2.down, launchForce);
                break;
            case 2: // right
                projectile.Launch(new Vector2(0.5f, -1), launchForce);
                break;
            default:
                break;
        }
    }
}