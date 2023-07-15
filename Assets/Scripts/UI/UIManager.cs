using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> hitpoints;
    [SerializeField]
    private List<GameObject> bombs;
    [SerializeField]
    private GameObject points;

    private TextMeshProUGUI pointsText;

    void Start()
    {
        pointsText = points.GetComponent<TextMeshProUGUI>();
        foreach (GameObject i in hitpoints)
        {
            i.SetActive(false);
        }

        SetPoints(0);
    }

    public void SetHP(int hp)
    {
        for (int i = 0; i < hitpoints.Count; ++i)
        {
            hitpoints[i].SetActive(i < hp);
        }
    }

    public void SetBombs(int count)
    {
        for (int i = 0; i < bombs.Count; ++i)
        {
            bombs[i].SetActive(i < count);
        }
    }

    public void SetPoints(int count)
    {
        pointsText.text = $"{count}";
    }
}
