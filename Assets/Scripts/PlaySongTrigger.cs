using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySongTrigger : MonoBehaviour
{
    public bool turnOn;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            if (turnOn)
                FindObjectOfType<PersistantAudioManager>().Play("Castle");
            else
            {
                FindObjectOfType<PersistantAudioManager>().TurnDown("Castle");
            }
        }
    }
}
