using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public float launchFrequency = 2f; // in seconds
    public GameObject projectilePrefab;
    public string directionString = "down";

    float timer;

    private void Start()
    {
        timer = launchFrequency; 
    }

    void Update()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, launchFrequency); 
        if (timer <= 0)
        {
            Launch();
            timer = launchFrequency;
        }
    }
    
    void Launch()
    {
        float launchForce = 500;
        if (directionString == "right")
        {
            GameObject projectileObject = Instantiate(projectilePrefab,
                transform.position + Vector3.right * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();

            projectile.Launch(Vector2.right, launchForce);
            projectileObject.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (directionString == "left")
        {
            GameObject projectileObject = Instantiate(projectilePrefab,
                transform.position + Vector3.left * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(Vector2.left, launchForce);
            projectileObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (directionString == "up")
        {
            GameObject projectileObject = Instantiate(projectilePrefab,
                transform.position + Vector3.up * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(Vector2.up, launchForce);
        }
        else
        {
            GameObject projectileObject = Instantiate(projectilePrefab,
                transform.position + Vector3.down * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(Vector2.down, launchForce);
            projectileObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }
}

















