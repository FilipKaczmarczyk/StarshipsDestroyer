using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public Transform[] cannon;

    public GameObject bulletPrefab;

    [SerializeField]
    public float bulletForce = 5f;

    private float fireRate = 0.5f;

    private float nextFire = -1f;

    void Update()
    {
        fireRate = GameController.FireRate;
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
            return;
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
                nextFire = fireRate;
            }
        }

    }

    void Shoot()
    {
       for(int i = 0; i < GameController.CannonsCount; i++)
       {
            GameObject bullet = Instantiate(bulletPrefab, cannon[i].position, cannon[i].rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(cannon[i].up * bulletForce, ForceMode2D.Impulse);
       }

    }
       

}