using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public GameObject Bullet;
    public float fireRate;
    private float timer;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            if (timer >= fireRate)
            {
                timer = 0;
                Shoot(); 
            }
        }
    }

    
    void Shoot()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 1.5f; 
        GameObject bulletInstance = Instantiate(Bullet, spawnPosition, transform.rotation);
        bulletInstance.SetActive(true);
    }
}