using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public Transform[] cannon;

    public GameObject bulletPrefab;

    AudioSource laserAudioData;

    [SerializeField]
    public float bulletForce = 5f;

    private float fireRate = 0.5f;

    private float nextFire = -1f;

    private bool x = false;

    void Start()
    {
        laserAudioData = GetComponent<AudioSource>();
    }

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
                if (x)
                {
                    Shoot();
                    nextFire = fireRate;
                }
                else
                {
                    Debug.Log("Cipa");
                    x = true;
                }
            }
        }

    }

    void Shoot()
    {
       laserAudioData.Play(0);
       for(int i = 0; i < GameController.CannonsCount; i++)
       {
            GameObject bullet = Instantiate(bulletPrefab, cannon[i].position, cannon[i].rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(cannon[i].up * bulletForce, ForceMode2D.Impulse);
       }

    }
       

}