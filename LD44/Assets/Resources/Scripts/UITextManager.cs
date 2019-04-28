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

    // This is the overlay that tells the player that the game is over
    public GameObject GameOverPanel;

    public void Start()
    {
        GameOverPanel.SetActive(false);
    }

    public void SetWave(int wave)
    {
        waveText.text = "Wave " + wave + " of 10";
        DoStartWaveAnimation(wave);
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

    public void SetEnemiesLeft(int wave)
    {
        enemiesLeftText.text = wave + " enemies left";
    }

    // This method is remotely triggered when a Game Over is assessed
    // by the GameManager
    public void OnGameOver()
    {
        GameOverPanel.SetActive(true);
    }
}
