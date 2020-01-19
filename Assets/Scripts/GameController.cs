using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    protected SpriteRenderer sr;

    protected Color dColor;

    private static float health = 10.0f;
    private static float maxHealth = 10.0f;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;
    private static int cannonsCount = 1;

    public static float Health { get => health; set => health = value ; }
    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static int CannonsCount { get => cannonsCount; set => cannonsCount = value; }

    public Text healthText;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        GameObject go = GameObject.FindWithTag("Player");
        sr = go.GetComponent<SpriteRenderer>();
        dColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health " + health;
        Debug.Log(fireRate);
        Debug.Log(CannonsCount); 
    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(instance.gameObject);
            //KillPlayer();
        }
        else
        {
            instance.CancelInvoke(nameof(ResetColor));
            instance.sr.color = Color.red;
            instance.Invoke(nameof(ResetColor), 0.05f);
        }

    }

    private void ResetColor()
    {
        sr.color = dColor;
    }



    public static void HealPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount);
    }

    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }

    public static void AttackSpeedChange(float speed)
    {

        if (fireRate < 0.2f)
        {
            if (cannonsCount <= 3)
            {
                fireRate = 0.5f;
                cannonsCount += 1;
            }
           
        }
        else
        {
            fireRate -= speed;
        }
        
    }


    public static void KillPlayer()
    {
        health = 10.0f;
        maxHealth = 10.0f;
        moveSpeed = 5f;
        fireRate = 0.5f;
        cannonsCount = 1;
        PlayerController.currentLevel = 0;
        DungeonCrawlerController.positionVisited.Clear();
        SceneManager.LoadScene("LevelMain"); 
    }

}



