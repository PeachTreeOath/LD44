using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextManager : Singleton<UITextManager>
{

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI startWaveText;
    public TextMeshProUGUI enemiesLeftText;

    private float totalAnimationTime = 2;

    public void SetWave(int wave)
    {
        waveText.text = "Wave " + wave + " of 10";
        DoStartWaveAnimation(wave);
    }

    public void SetEnemiesLeft(int wave)
    {
        enemiesLeftText.text = wave + " enemies left";
    }

    public void DoStartWaveAnimation(int wave)
    {
        startWaveText.text = "-Wave " + wave + "-";
        StartCoroutine("StartWaveTextCR");
    }

    IEnumerator StartWaveTextCR()
    {
        float timeToAnimate = 0;
        while (timeToAnimate < totalAnimationTime)
        {
            timeToAnimate += Time.deltaTime;
            startWaveText.color = new Color(startWaveText.color.r, startWaveText.color.g, startWaveText.color.b, totalAnimationTime - timeToAnimate);
            yield return null;
        }
    }
}
