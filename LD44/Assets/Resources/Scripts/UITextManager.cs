using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextManager : Singleton<UITextManager>
{

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesLeftText;

    public void SetWave(int wave)
    {
        waveText.text = "Wave " + wave + " of 10";
    }

    public void SetEnemiesLeft(int wave)
    {
        enemiesLeftText.text = wave + " enemies left";
    }
}
