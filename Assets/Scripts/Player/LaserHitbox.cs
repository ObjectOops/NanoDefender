using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitbox : MonoBehaviour
{
    private BoxCollider2D hitbox;

    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(transform.parent.gameObject);
    }
}
