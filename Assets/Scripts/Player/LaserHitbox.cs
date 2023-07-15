using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitbox : MonoBehaviour
{
    public GameManager gameManager;

    private BoxCollider2D hitbox;

    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameManager.AddPoints(1);
        Destroy(collision.gameObject);
        Destroy(transform.parent.gameObject);
    }
}
