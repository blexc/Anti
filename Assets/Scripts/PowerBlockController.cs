using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlockController : MonoBehaviour
{
    public bool isPowered;
    public bool power { get { return isPowered; } }
    public bool canSwitch { get; private set; }

    float startTimer = 1f;
    float timer = 0;
    Animator animator;
    BoxCollider2D boxCollider2D;
    Color tempColor;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        tempColor = GetComponent<SpriteRenderer>().color;
        canSwitch = true;

        // set power (two is purposeful)
        SwitchPower();
        SwitchPower();
    }

    private void Update()
    {
        if (timer <= 0)
        {
            animator.SetBool("Rainbow", false);
            canSwitch = true;
        }
        else
        {
            animator.SetBool("Rainbow", true);
            timer = Mathf.Clamp(timer - Time.deltaTime, 0, startTimer);
        }
    }

    public void SwitchPower()
    {
        if (isPowered)
        {
            boxCollider2D.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            tempColor.a = 0.5f;
            animator.SetBool("isPowered", false);
            isPowered = false;
        }
        else
        {
            boxCollider2D.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Ground");
            tempColor.a = 1f;
            animator.SetBool("isPowered", true);
            isPowered = true;
        }

        GetComponent<SpriteRenderer>().color = tempColor;
    }

    public void Rainbow()
    {
        SwitchPower();
        canSwitch = false; // cant control for 3 seconds
        timer = startTimer;
    }
}
