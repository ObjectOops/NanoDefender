using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField]
    private GameObject hitbox;
    [SerializeField]
    private float speed;

    void Start()
    {
        hitbox.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * (IsFlipped() ? -1 : 1), 0);
    }

    public void StopLaser()
    {
        Destroy(gameObject);
    }

    private bool IsFlipped()
    {
        return transform.rotation.eulerAngles[1] == 180;
    }
}
