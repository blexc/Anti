using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossRoom : MonoBehaviour
{
    public GameObject[] enemy;
    public GameObject[] blockades;

    PlayerController player;
    bool playerEntered;

    private void Start()
    {
        playerEntered = false;
    }

    private void Update()
    {
        if (playerEntered)
        {
            bool roomCleared = true;
            for (int i = 0; i < enemy.Length; i++)
            {
                if (enemy[i].GetComponent<EnemyController>().Health > 0)
                {
                    roomCleared = false;
                    break;
                }
            }
            if (roomCleared == true)
            {
                for (int i = 0; i < blockades.Length; i++)
                {
                    player.isRemoved(blockades[i].gameObject.name);
                    blockades[i].GetComponent<RoomBlockade>().Open();
                }
                player.isRemoved(this.gameObject.name);
                FindObjectOfType<PersistantAudioManager>().Play("Castle");
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            playerEntered = true;
            FindObjectOfType<PersistantAudioManager>().Play("Climate");
            for (int i = 0; i < blockades.Length; i++)
            {
                blockades[i].GetComponent<RoomBlockade>().Close();
            }
        }
    }
}
