using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    float movementSpeed = 4f;

    public Rigidbody2D rb;

    public Camera cam;

    private Vector2 movement = Vector3.zero;

    Vector2 mousePosition;

    public TextMeshProUGUI level;

    public static int currentLevel = 0;

    public static int collectedAmount = 0;

    public PolygonCollider2D pc;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //pc = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        movementSpeed = GameController.MoveSpeed;

        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
        
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        level.text = "STAGE " + currentLevel;

        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "EnemyPiece")
        {
            Physics2D.IgnoreCollision(pc, coll.collider);
        }
    }

}
