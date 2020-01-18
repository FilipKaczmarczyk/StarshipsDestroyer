using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public Transform cannon;

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
        GameObject bullet = Instantiate(bulletPrefab, cannon.position, cannon.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(cannon.up * bulletForce, ForceMode2D.Impulse);
    }

}