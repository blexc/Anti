using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region variables
    public int health { get { return currentHealth; } }
    public int chipCount { get { return currentChipCount; } }
    public int musicNoteCount { get { return currentMusicNoteCount; } }
    public Vector2 velocity { get { return rigidbody2d.velocity; } }
    public Vector3 position { get { return transform.position; } }

    // player data to save/ load
    public GameObject[] removeable;
    public bool[] isItemRemoveable = new bool[100];
    public int maxHealth;
    int currentChipCount = 0;
    int currentMusicNoteCount = 0;
    int currentHealth;
    public bool hasSword;
    public bool hasZap;
    public bool hasHovering;
    Vector3 pos;

    public bool debugMode = false;
    public float speed = 8;
    public float startTimeBtwAttack = 0.5f;
    public float startHoverTime = 2;
    public float minJumpTime = 0.15f;
    public float maxJumpTime = 1f;
    public float startJumpForce = 1000f;
    public Transform attackPos;
    public LayerMask whatIsHittable;
    public LayerMask whatIsGround;
    public float attackRange = 1f;
    public int damage = 1;
    public float timeInvincible = 2.0f;

    Vector2 currentVelocity;
    bool isInvincible;
    float invincibleTimer;
    float jumpTime;
    float hoverTime;
    public bool hoverAllowed; // to prevent infinite hover
    float timeBtwAttack;
    float jumpForce;
    float defaultGravity;
    float startOneWayPlatTime = 0.5f;
    float oneWayPlatTime;
    float startSpringTime = 0.5f;
    float springTime;

    bool isStone = false;
    float startTransitionTimer = 10f;
    float transitionTimer;

    // input
    float horizontal;
    float vertical;
    bool jumpPressed;
    bool jumpReleased;
    bool hoverPressed;
    bool hoverHeld;

    Rigidbody2D rigidbody2d;
    BoxCollider2D boxCollider2D;
    Animator animator;
    GameObject currentRoom;
    PowerBlockController[] powerBlockController;
    PowerBlockMask[] powerBlockMask;
    Vector2 lookDirection = new Vector2(1, 0);
    #endregion

    private void Awake()
    {
        removeable = GameObject.FindGameObjectsWithTag("Removeable");
        LoadPlayer();
    }

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UIHealth.instance.health = currentHealth;
        UIHealth.instance.numOfHearts = maxHealth;
        jumpTime = maxJumpTime;
        defaultGravity = rigidbody2d.gravityScale;
    }

    void Update()
    {
        #region input, movement and animation
        if (!isStone)
        {
            jumpPressed = Input.GetButtonDown("Jump");
            jumpReleased = Input.GetButtonUp("Jump");
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
        else
        {
            transitionTimer = Mathf.Clamp(transitionTimer - Time.deltaTime, 0, startTransitionTimer);
            if (transitionTimer <= 0)
            {
                SceneManager.LoadScene("Credits");
            }
        }

        currentVelocity = new Vector2(horizontal * speed, 0f);
        rigidbody2d.velocity = currentVelocity;

        Vector2 move = new Vector2(horizontal, 0);
        if (!Mathf.Approximately(move.x, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isInvinsible", isInvincible);
        animator.SetFloat("Speed", move.magnitude);
        #endregion

        #region special movement
        HandleInvisibility();
        HandleZap();

        if (hasHovering)
        {
            hoverPressed = Input.GetButtonDown("Hover");
            hoverHeld = Input.GetButton("Hover");
            if (IsGrounded())
                hoverAllowed = true;
            if (hoverPressed && hoverAllowed)
                hoverTime = startHoverTime;
        }

        if (IsGrounded())
        {
            if (oneWayPlatTime == 0)
                gameObject.layer = LayerMask.NameToLayer("Player");

            if (jumpPressed)
            {
                if (Input.GetAxis("Vertical") < 0 && oneWayPlatTime >= 0)
                {
                    gameObject.layer = LayerMask.NameToLayer("OneWay Platform");
                    oneWayPlatTime = startOneWayPlatTime;
                }
                else
                {
                    jumpTime = 0;
                    jumpForce = startJumpForce;
                }
            }
        }
        oneWayPlatTime = Mathf.Clamp(oneWayPlatTime - Time.deltaTime, 0, startOneWayPlatTime);
        if (jumpReleased)
        {
            if (jumpTime > minJumpTime)
                jumpTime = maxJumpTime;
            else
                jumpTime = maxJumpTime - minJumpTime;
        }
        #endregion

        #region attacking and springing
        if (timeBtwAttack <= 0 && hasSword)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Collider2D[] thingsToHit = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsHittable);
                animator.SetTrigger("Swing");

                if (thingsToHit.Length > 0)
                {
                    FindObjectOfType<AudioManager>().RandomizePitch("SwingHit", 0.8f, 0.85f);
                    FindObjectOfType<AudioManager>().Play("SwingHit");
                }
                else
                {
                    FindObjectOfType<AudioManager>().RandomizePitch("SwingMiss", 0.65f, 0.75f);
                    FindObjectOfType<AudioManager>().Play("SwingMiss");
                }

                for (int i = 0; i < thingsToHit.Length; i++)
                {
                    if (thingsToHit[i].tag == "Spring")
                    {
                        springTime = startSpringTime;
                        jumpForce = startJumpForce * 1.5f;
                    }
                    else if (thingsToHit[i].tag == "Boss")
                    {
                        thingsToHit[i].GetComponent<BossController>().TakeDamage(damage);
                    }
                    else if (thingsToHit[i].tag == "Enemy")
                    {
                        thingsToHit[i].GetComponent<EnemyController>().TakeDamage(damage, Mathf.RoundToInt(lookDirection.x));
                    }
                    else if (thingsToHit[i].tag == "Cloud")
                    {
                        SavePlayer();
                        UIMisc.instance.ShowSave();
                    }
                    else
                    {
                        Debug.LogError("thing hit " + thingsToHit[i].tag + " not taken into account");
                    }
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
            timeBtwAttack -= Time.deltaTime;
        #endregion

    }

    private void FixedUpdate()
    {
        if (hoverHeld && hoverTime > 0)
        {
            hoverTime -= Time.fixedDeltaTime;
            jumpTime = maxJumpTime;
            rigidbody2d.gravityScale = 0;
            hoverAllowed = false;
        }
        else
        {
            rigidbody2d.gravityScale = defaultGravity;
            if (rigidbody2d.velocity.y < 0)
                rigidbody2d.AddForce(Vector2.down * 400);
        }

        if (springTime > 0)
        {
            rigidbody2d.AddForce(Vector2.up * jumpForce);
            jumpForce = Mathf.Max(450f, jumpForce - jumpForce / 50);
            springTime -= Time.fixedDeltaTime;
        }
        else if (jumpTime < maxJumpTime)
        {
            rigidbody2d.AddForce(Vector2.up * jumpForce);
            jumpForce = Mathf.Max(450f, jumpForce - jumpForce / 50);
            jumpTime += Time.fixedDeltaTime;
        }
    }

    public void ChipCollected()
    {
        currentChipCount++;
    }

    public void MusicNoteCollected()
    {
        currentMusicNoteCount++;
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealth.instance.health = currentHealth;
        UIHealth.instance.numOfHearts = maxHealth;

        if (currentHealth <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpgradeHealth()
    {
        maxHealth++;
        currentHealth = maxHealth;
        UIHealth.instance.numOfHearts = maxHealth;
        UIHealth.instance.health = currentHealth;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center + Vector3.down,
            boxCollider2D.bounds.size + Vector3.down, 0, Vector2.down, 0.1f, whatIsGround);
        return raycastHit.collider != null;
    }

    private void HandleInvisibility()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0) isInvincible = false;
        }
    }

    private void HandleZap()
    {
        LayerMask layerMask = LayerMask.GetMask("PowerBlockMask");
        if (hasZap)
        {
            bool pressed = Input.GetButtonDown("Fire2");
            if (pressed) 
            {
                // make sure none of the blocks switch if player is near one
                for (int i = 0; i < powerBlockMask.Length; i++)
                {
                    if (powerBlockMask[i].IsNearPlayer())
                    {
                        return;
                    }
                }

                // check if there is at least one block that is not rainbow
                int blocksThatCanSwitch = 0;
                for (int i=0; i < powerBlockController.Length; i++)
                {
                    if (powerBlockController[i].canSwitch)
                    {
                        blocksThatCanSwitch++;
                    }
                }

                if (blocksThatCanSwitch <= 0)
                {
                    return;
                }

                if (powerBlockController.Length > 0)
                {
                    FindObjectOfType<AudioManager>().Play("BossProjectile");
                }

                for (int i = 0; i < powerBlockController.Length; i++)
                {
                    if (powerBlockController[i].canSwitch) 
                        powerBlockController[i].SwitchPower();
                }
            }
        }
    }

    public void UpdateCurrentRoom(GameObject aroom)
    {
        currentRoom = aroom;
        powerBlockController = aroom.GetComponentsInChildren<PowerBlockController>();
        powerBlockMask = aroom.GetComponentsInChildren<PowerBlockMask>();
    }

    public void NewPlayer()
    {
        currentHealth = maxHealth;
        UIHealth.instance.health = currentHealth;
        UIHealth.instance.numOfHearts = maxHealth;
        SaveSystem.SavePlayer(this);
        LoadPlayer();
    }

    public void SavePlayer()
    {
        currentHealth = maxHealth;
        UIHealth.instance.health = currentHealth;
        UIHealth.instance.numOfHearts = maxHealth;
        SaveSystem.SavePlayer(this);
        UIMisc.instance.ShowSave();
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            hasSword = data.hasSword;
            hasZap = data.hasZap;
            hasHovering = data.hasHovering;
            currentChipCount = data.chipCount;
            currentMusicNoteCount = data.musicNoteCount;
            maxHealth = data.maxHealth;
            currentHealth = data.health;
            pos.x = data.pos[0];
            pos.y = data.pos[1];
            pos.z = data.pos[2];

            for (int i = 0; i < removeable.Length; i++)
            {
                isItemRemoveable[i] = data.isItemRemoveable[i];
                if (isItemRemoveable[i])
                {
                    removeable[i].SetActive(false);
                }
            }

            currentHealth = maxHealth;
            UIHealth.instance.numOfHearts = maxHealth;
            UIHealth.instance.health = currentHealth;

            transform.position = pos;
        }
        else
        {
            NewPlayer();
        }
    }

    // check if any removable items have been removed
    public void isRemoved(string name)
    {
        for (int i=0; i < removeable.Length; i++)
        {
            if (removeable[i].name == name)
            {
                isItemRemoveable[i] = true;
                return;
            }
        }
        Debug.LogError("no item found called, " + name);
    }

    public void TurnToStone()
    {
        speed = 0;
        animator.SetTrigger("Solidify");
        hasHovering = false;
        hasSword = false;
        hasZap = false;
        startJumpForce = 0;
        animator.SetBool("Solid", true);
        isStone = true;
        transitionTimer = startTransitionTimer;
    
    }

    public bool HasStar()
    {
        return currentChipCount >= 7 && currentMusicNoteCount >= 8 && maxHealth >= 10;
    }
}
