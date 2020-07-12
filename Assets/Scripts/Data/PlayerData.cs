using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool hasSword;
    public bool hasZap;
    public bool hasHovering;
    public int chipCount;
    public int musicNoteCount;
    public int maxHealth;
    public int health;
    public float[] pos = new float[3];
    public int sceneIndex;
    public bool[] isItemRemoveable = new bool[100];

    public PlayerData(PlayerController player)
    {
        hasSword = player.hasSword;
        hasZap = player.hasZap;
        hasHovering = player.hasHovering;
        chipCount = player.chipCount;
        musicNoteCount = player.musicNoteCount;
        maxHealth = player.maxHealth;
        health = player.health;
        pos[0] = player.position.x;
        pos[1] = player.position.y;
        pos[2] = player.position.z;
        for (int i = 0; i < player.isItemRemoveable.Length; i++)
        {
            isItemRemoveable[i] = player.isItemRemoveable[i];
        }
    }
}
