using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlockMask : MonoBehaviour
{
    bool isNearPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            isNearPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            isNearPlayer = false;
        }
    }

    public bool IsNearPlayer()
    {
        return isNearPlayer;
    }
}
