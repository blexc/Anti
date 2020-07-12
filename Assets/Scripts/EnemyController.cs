using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int Health { get { return health; } }

    public float maxSpeed;
    public float startTurnTime;
    public int maxHealth;
    public float startDazedTime;
    public LayerMask whatIsGround;
    public GameObject heartPrefab;
    //public Animation hurtAnimation;

    Rigidbody2D rb2d;
    BoxCollider2D boxCollider2D;
    int direction = 1;
    int launchDirection;
    int health;
    float turnTime;
    float dazedTime;
    float speed;
    float startLaunchHeight = 500;
    float launchHeight;
    float launchDistance = 100;
    Vector3 initialPosition;

    Animator animator;
    SpriteRenderer enemySprite;
    int initialLayer;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        turnTime = startTurnTime;
        health = maxHealth;
        speed = maxSpeed;
        dazedTime = 0;
        animator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        initialPosition = rb2d.position;
        initialLayer = gameObject.layer;
    }

    void Update()
    {
        if (IsGrounded())
        {
            rb2d.gravityScale = 0;
            speed = maxSpeed;
        }
        else
        {
            rb2d.gravityScale = 20;
            speed = 0;
        }

        if (IsClippedFromBelow())
        {
            Vector3 position = rb2d.transform.position;
            position.y += 0.1f;
            rb2d.transform.position = position;
        }

        if (dazedTime > 0)
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
            animator.SetBool("Hit", true);
        }
        else
        {
            speed = maxSpeed;
            if (turnTime < 0)
            {
                direction = -direction;
                turnTime = startTurnTime;
            }
            animator.SetBool("Hit", false);
            turnTime -= Time.deltaTime;
        }

        if (health <= 0)
        {
            gameObject.layer = 0;
            Color c = enemySprite.material.color;
            c.a -= Time.deltaTime;
            enemySprite.material.color = c;
            
            if (c.a <= 0)
            {
                int chance = Random.Range(0, 19); // 1 and 20 chance to spawn
                if (chance == 0)
                {
                    Instantiate(heartPrefab, rb2d.position + Vector2.up, Quaternion.identity);
                }
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.layer = initialLayer;
        }
        
        animator.SetFloat("Move X", direction);
    }

    private void FixedUpdate()
    {
        if (!IsAtWall())        {
            if (dazedTime > 0)
            {
                Vector2 launch = new Vector2(launchDirection * launchDistance, launchHeight);
                rb2d.AddForce(launch);
                launchHeight = Mathf.Max(-startLaunchHeight, launchHeight - 20);
            }
        }

        Vector2 velocity = new Vector2(direction * speed, 0f);
        rb2d.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        PowerBlockController powerBlock = collision.gameObject.GetComponent<PowerBlockController>();
        if (powerBlock != null && powerBlock.power)
        {
            TakeDamage(maxHealth, 0);
        }
        else if (player != null && dazedTime <= 0 && enemySprite.material.color.a >= 0.9f)
        {
            player.ChangeHealth(-1);
        }
    }

    public void TakeDamage(int amount, int playerDirection)
    {
        dazedTime = startDazedTime;
        health -= amount;
        launchDirection = playerDirection;
        launchHeight = startLaunchHeight;
    }

    public void Respawn()
    {
        health = maxHealth;
        
        gameObject.SetActive(true);

        rb2d.position = initialPosition;
        gameObject.layer = initialLayer;
        turnTime = startTurnTime;
        speed = maxSpeed;
        dazedTime = -1;
        direction = 1;

        Color c = enemySprite.material.color;
        c.a = 1f;
        enemySprite.material.color = c;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, 
            boxCollider2D.bounds.size, 0, Vector2.down, 0.25f, whatIsGround);
        return raycastHit.collider != null;
    }

    private bool IsClippedFromBelow()
    {
        Vector2 origin = boxCollider2D.bounds.center;
        Vector2 size = boxCollider2D.bounds.size;
        RaycastHit2D raycastHit = Physics2D.BoxCast(origin, size, 0, Vector2.down, 0.1f, whatIsGround);
        return raycastHit.collider != null;
    }

    private bool IsAtWall()
    {
        bool isAtWall = false;
        float distance = 0.25f;
        Color color = Color.green;

        Vector2 originLeft = rb2d.position + Vector2.left * distance;
        Vector2 originRight = rb2d.position + Vector2.right * distance;
        Vector2 size = boxCollider2D.bounds.size;

        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(originLeft, size, 0, Vector2.left, distance, whatIsGround);
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(originRight, size, 0, Vector2.right, distance, whatIsGround);

        Collider2D leftCollider = raycastHitLeft.collider;
        Collider2D rightCollider = raycastHitRight.collider;

        if (raycastHitLeft.collider != null)
        {
            //if (leftCollider != null) Debug.Log(gameObject.name + " " + leftCollider.name);
            color = Color.red;
            direction = 1;
            turnTime = startTurnTime;
            isAtWall = true;
        }
        else if (raycastHitRight.collider != null)
        {
            //if (rightCollider != null) Debug.Log(gameObject.name + " " + rightCollider.name);
            color = Color.red;
            direction = -1;
            turnTime = startTurnTime;
            isAtWall = true;
        }

        //Debug.DrawRay(rb2d.position + Vector2.left * distance, rb2d.transform.TransformDirection(Vector3.left) * distance, color);
        //Debug.DrawRay(rb2d.position + Vector2.right * distance, rb2d.transform.TransformDirection(Vector3.right) * distance, color);

        return isAtWall;
    }
}












