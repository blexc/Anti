using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public GameObject[] blockades;
    public BossController bossController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            for (int i = 0; i < blockades.Length; i++)
            {
                blockades[i].GetComponent<RoomBlockade>().Close();
                bossController.AwakenState();
                gameObject.SetActive(false);
            }
        }
    }
}
