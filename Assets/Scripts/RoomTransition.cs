using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomTransition : MonoBehaviour
{
    public GameObject virtualCamera;
    private EnemyController[] enemyController;

    private void Start()
    {
        enemyController = GetComponentsInChildren<EnemyController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            virtualCamera.SetActive(true);
            player.UpdateCurrentRoom(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            for (int i = 0; i < enemyController.Length; i++)
            {
                enemyController[i].Respawn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.SetActive(false);
        }
    }
}
