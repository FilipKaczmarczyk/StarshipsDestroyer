using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyState
{
    Afk,

    Idle,

    Aim,

    Follow,

    Die,

    Attack
};

public enum EnemyType
{
    Melee,

    Ranged
};

[RequireComponent(typeof(Explodable))]
public class EnemyController : MonoBehaviour
{
    GameObject player;

    public Transform cannon;

    public EnemyState currentState = EnemyState.Idle;
    public EnemyType enemyType;

    public float range = 10.0f;
    public float speed;
    public float coolDown;
    public float meleeRange = 0.82f;
    public int damage;
    public int explosionDamage = 5;

    public int health = 10;

    public float bulletForce = 5f;

    private bool coolDownAttack = false;

    public bool notInRoom = true;

    public GameObject bulletPrefab;

    protected SpriteRenderer sr;

    public PolygonCollider2D pc;

    protected Color dColor;

    private Explodable _explodable;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        pc = GetComponent<PolygonCollider2D>();
        dColor = sr.color;

    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _explodable = GetComponent<Explodable>();
    }

    void Update()
    {
        switch (currentState)
        {
            case (EnemyState.Afk):
                Afk();
                break;
            case (EnemyState.Idle):
                Idle();
                break;
            case (EnemyState.Aim):
                Aim();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Die):
                Die();
                break;
            case (EnemyState.Attack):
                Attack();
                break;
        }
        if (health > 0)
        {
            if (!notInRoom)
            {
                if (enemyType == EnemyType.Melee)
                {
                    currentState = EnemyState.Follow;
                }
                else if (enemyType == EnemyType.Ranged)
                {
                    if (!isPlayerInRange(range))
                    {
                        currentState = EnemyState.Follow;
                    }
                    else if (isPlayerInRange(range))
                    {
                        currentState = EnemyState.Aim;
                    }
                }
            }
            else
            {
                currentState = EnemyState.Afk;
            }
        }
        else
        {
            currentState = EnemyState.Die;
        }

    }

    private bool isPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }


    public void DamageEnemy(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            currentState = EnemyState.Die;
            Death();
        }
        else
        {
            CancelInvoke(nameof(ResetColor));
            sr.color = Color.red;
            Invoke(nameof(ResetColor), 0.05f);
        }
    }

    private void ResetColor()
    {
        sr.color = dColor;
    }

    void Afk()
    {

    }

    void Aim()
    {
        float rotationSpeed = 10f;
        float offset = -90f;
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        Attack();
    }

    void Follow()
    {
        float rotationSpeed = 10f;
        float offset = -90f;
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        if (!coolDownAttack)
        {   
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<BulletController>().GetPlayer(player.transform);
            bullet.GetComponent<BulletController>().isEnemyBullet = true;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(cannon.up * bulletForce, ForceMode2D.Impulse);
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    void Die()
    {
        pc.enabled = false;
        StartCoroutine(DestroyEnemy());
    }

    public void Death()
    {
        _explodable.explode();
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
    }

    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCourutine());
    }

    void Idle()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "EnemyPiece")
        {
            Physics2D.IgnoreCollision(pc, coll.collider);
        }
        else if (coll.gameObject.tag == "Player")
        {
            GameController.DamagePlayer(explosionDamage);
            health = 0;
            Death();
        }
    }


}