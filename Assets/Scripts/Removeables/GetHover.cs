using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHover : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            FindObjectOfType<AudioManager>().Play("Collectible");
            controller.hoverAllowed = true;
            controller.hasHovering = true;
            UIMisc.instance.ExplainHover();
            controller.isRemoved(this.gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
