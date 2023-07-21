using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackWaveEndManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text waveText, bonusText;
    public Transform humanBonus;
    public GameObject humanUI;
    
    public GameManager gameManager;
    
    public void EndWave(int wave) {
        waveText.text = $"ATTACK WAVE {wave} COMPLETED";
        int bonusPoints = wave * 100;
        UIManager.instance.AddPoints(bonusPoints);
        
        bonusText.text = $"BONUS X {bonusPoints}";
        
        for(int i = 0; i < humanBonus.childCount; i++) {
            Transform t = humanBonus.GetChild(i);
            Destroy(t.gameObject);
        }
        
        int humans = FindObjectsOfType<Human>().Length;
        for(int i = 0; i < humans; i++) {
            Instantiate(humanUI, Vector3.zero, Quaternion.identity, humanBonus);
        }
    }
}
