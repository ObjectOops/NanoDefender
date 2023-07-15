using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int hitpoints, bombs;
    [SerializeField]
    private float maxY, minY, posY, deltaY;
    [SerializeField]
    private GameObject laser;
    [SerializeField]
    private Vector2 laserSpawnOffset;
    [SerializeField]
    private GameObject ui, game;

    private UIManager uiManager;
    private GameManager gameManager;
    private new SpriteRenderer renderer;

    [HideInInspector]
    public Direction direction = Direction.Right;
    [HideInInspector]
    public bool accelerating, firing, usedBomb;

    private float axisX, axisY;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        uiManager = ui.GetComponent<UIManager>();
        gameManager = game.GetComponent<GameManager>();
        uiManager.SetHP(hitpoints);
        uiManager.SetBombs(bombs);
    }

    void Update()
    {
        axisY = Input.GetAxisRaw("Vertical");
        axisX = Input.GetAxisRaw("Horizontal");
        accelerating = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space);
        firing = Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Return);
        usedBomb = Input.GetKeyDown(KeyCode.B) && bombs > 0;
        posY = Mathf.Clamp(posY + deltaY * axisY, minY, maxY);

        UpdatePositionY();
        UpdateOrientation();
        if (firing)
        {
            Fire();
        }
        if (usedBomb)
        {
            --bombs;
            uiManager.SetBombs(bombs);
            gameManager.Bomb();
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
        newLaser.transform.GetChild(0).GetComponent<LaserHitbox>().gameManager = gameManager;
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

    public void TakeDamage()
    {
        --hitpoints;
        uiManager.SetHP(hitpoints);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            --hitpoints;
            uiManager.SetHP(hitpoints);
            Destroy(collision.gameObject);
            if (hitpoints < 1)
            {
                Destroy(gameObject);
            }
        }
    }
}

public enum Direction
{
    Left, 
    Right
}
