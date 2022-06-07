using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 1f;
    Rigidbody2D enemyRB;
    BoxCollider2D boxCollider;

    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        enemyRB.velocity = new Vector2(speed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        speed = -speed;
        FlipEnemyFace();
    }

    private void FlipEnemyFace()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRB.velocity.x)), transform.localScale.y);
    }
}
