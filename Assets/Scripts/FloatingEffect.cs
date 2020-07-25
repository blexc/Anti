using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    Transform trans;
    float time;
    public float period;
    public float amp;

    private void Start()
    {
        trans = gameObject.GetComponent<Transform>();
        period *= Mathf.PI;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            time += Time.deltaTime;
            if (time > 1000) time = 0;
            trans.position += Vector3.up * amp * Mathf.Sin(period * time);
        }
    }
}
