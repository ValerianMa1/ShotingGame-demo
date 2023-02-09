using UnityEngine.UI;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    Text waveText;



    void Awake() 
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        waveText = GetComponentInChildren<Text>();
    }

    void OnEnable() 
    {
        waveText.text = "- WAVE " + EnemyManager.instance.WaveNumber + " -";    
    }
}
