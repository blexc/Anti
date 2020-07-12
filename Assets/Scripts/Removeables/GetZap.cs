using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetZap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            FindObjectOfType<AudioManager>().Play("Collectible");
            controller.hasZap = true;
            UIMisc.instance.ExplainZap();
            controller.isRemoved(this.gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
