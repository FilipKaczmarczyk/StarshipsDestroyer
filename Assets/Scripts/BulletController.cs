using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool isEnemyBullet = false;

    public Vector2 lastPos;
    public Vector2 curPos;
    public Vector2 playerPos;

    public float lifetime = 2.0f;

    void Awake()
    {
        Destroy(this.gameObject, lifetime);
    }

    void Start()
    {

    }

    void Update()
    {
        if (isEnemyBullet)
        {
            //curPos = transform.position;
           // transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
           // if(curPos == lastPos)
           // {
                //Destroy(gameObject);
           // }
           // lastPos = curPos;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && (!isEnemyBullet))
        {
            collision.gameObject.GetComponent<EnemyController>().DamageEnemy(5);
            //collision.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        if (collision.tag == "Player" && (isEnemyBullet))
        {
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }

        if (collision.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }
}
