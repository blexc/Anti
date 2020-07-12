using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    public static SetResolution instance;

    public int width;
    public int height;
    public bool fullscreen;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
       Screen.SetResolution(width, height, fullscreen);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
        {
            fullscreen = !fullscreen;
            Screen.SetResolution(width, height, fullscreen);
        }
    }
}
