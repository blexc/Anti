using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartUpgradeCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            FindObjectOfType<AudioManager>().Play("Collectible");
            controller.UpgradeHealth();
            controller.isRemoved(this.gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
