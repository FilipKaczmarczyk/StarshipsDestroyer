using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    private static float health = 10.0f;
    private static float maxHealth = 10.0f;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;

    public static float Health { get => health; set => health = value ; }
    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }

    public Text healthText;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health " + health;
    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            KillPlayer();
        }

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
        fireRate -= speed;
    }


    public static void KillPlayer()
    {
        health = 10.0f;
        maxHealth = 10.0f;
        moveSpeed = 5f;
        fireRate = 0.5f;
        PlayerController.currentLevel = 0;
        DungeonCrawlerController.positionVisited.Clear();
        SceneManager.LoadScene("LevelMain"); 
    }

}



