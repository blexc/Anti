using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float scrollSpeed;
    public RectTransform targetRect;
    public RectTransform[] textBoxes;

    void Update()
    {
        // textBoxes[0] should be the last thing to show, and
        // it will stop once it reaches the target position, and
        // the player will be able to transition to title
        if (textBoxes[0].position.y > targetRect.position.y)
        {
            for (int i=0; i < textBoxes.Length; i++)
            {
                textBoxes[i].position += Vector3.down * scrollSpeed; 
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                FindObjectOfType<PersistantAudioManager>().TurnDown("Anti");
                SceneManager.LoadScene("Title");
            }
        }
    }
}
