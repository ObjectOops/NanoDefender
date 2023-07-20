using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackWaveEndManager : MonoBehaviour
{
    public TMP_Text waveText;
    public TMP_Text bonusText;
    
    public void EndWave(int wave) {
        waveText.text = $"ATTACK WAVE {wave} COMPLETED";
        bonusText.text = $"BONUS X {wave * 100}";
    }
}
