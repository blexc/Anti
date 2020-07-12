using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    Transform trans;
    float time;
    public float period = 1f;
    public float amp = 0.005f;

    private void Start()
    {
        trans = gameObject.GetComponent<Transform>();
        period *= Mathf.PI;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (Time.timeScale != 0)
        {
            trans.position += Vector3.up * amp * Mathf.Sin(period * time);
        }
    }
}
