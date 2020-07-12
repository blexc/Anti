using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNoteCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.MusicNoteCollected();
            FindObjectOfType<AudioManager>().Play("Collectible");
            controller.isRemoved(this.gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
