using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public static UIHealth instance { get; private set; }
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for(int i=0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
            hearts[i].enabled = (i < numOfHearts) ? true : false;
        }    
    }
}
