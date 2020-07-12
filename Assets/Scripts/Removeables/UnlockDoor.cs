using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject door;
    public int chipReq;
    public int noteReq;

    bool diminish = false;

    SpriteRenderer doorSprite;
    PlayerController player;

    void Awake()
    {
        doorSprite = door.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (diminish)
        {
            Color c = doorSprite.material.color;
            c.a -= Time.deltaTime;
            doorSprite.material.color = c;

            if (c.a <= 0)
            {
                player.isRemoved(doorSprite.gameObject.name);
                door.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        player = other.gameObject.GetComponent<PlayerController>();
        if (player != null && player.chipCount >= chipReq && player.musicNoteCount >= noteReq)
        {
            FindObjectOfType<AudioManager>().Play("Unlocked");
            diminish = true;
        }
    }
}
