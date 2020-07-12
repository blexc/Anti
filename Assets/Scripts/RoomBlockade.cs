using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBlockade : MonoBehaviour
{
    public bool openByDefault = false;
    SpriteRenderer spriteRenderer;
    Vector2 finalSize;
    Vector2 currentSize;
    float startTime = 3f;
    float time = 0f;
    float direction; // pos if closing, negative if opening

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        finalSize = spriteRenderer.size;
        currentSize = spriteRenderer.size;
        
        if (openByDefault == false)
        {
            currentSize = new Vector2(0, 1);
            spriteRenderer.size = currentSize;
        }
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time > 0)
        {
            float width = Mathf.Clamp(spriteRenderer.size.x + 0.2f * direction, 0, finalSize.x);
            currentSize = new Vector2(width, 1);
            spriteRenderer.size = currentSize;
        }
    }

    public void Close()
    {
        time = startTime;
        direction = 1;
    }

    public void Open()
    {
        time = startTime;
        direction = -1;
    }
}
