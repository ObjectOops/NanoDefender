using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxY, minY, posY, deltaY;
    [SerializeField]
    private GameObject laser;
    [SerializeField]
    private Vector2 laserSpawnOffset;
    [SerializeField]
    private float laserSpeed;

    private new SpriteRenderer renderer;

    [HideInInspector]
    public Direction direction = Direction.Right;
    [HideInInspector]
    public bool accelerate;

    private float axisX, axisY;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        axisY = Input.GetAxisRaw("Vertical");
        axisX = Input.GetAxisRaw("Horizontal");
        accelerate = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space);
        bool fireNow = Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Return);
        posY = Mathf.Clamp(posY + deltaY * axisY, minY, maxY);

        UpdatePositionY();
        UpdateOrientation();
        if (fireNow)
        {
            Fire();
        }
    }

    private void UpdatePositionY()
    {
        transform.position = new Vector2(transform.position.x, posY);
    }

    private void UpdateOrientation()
    {
        if (axisX != 0)
        {
            direction = axisX < 0 ? Direction.Left : Direction.Right;
        }
        renderer.flipX = direction == Direction.Left;
    }

    private void Fire()
    {
        GameObject newLaser = Instantiate(laser);
        int dir = direction == Direction.Right ? 1 : -1;
        newLaser.transform.position = new Vector2(
            transform.position.x + laserSpawnOffset.x * dir, 
            transform.position.y + laserSpawnOffset.y
        );
        if (direction == Direction.Left)
        {
            newLaser.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}

public enum Direction
{
    Left, 
    Right
}
