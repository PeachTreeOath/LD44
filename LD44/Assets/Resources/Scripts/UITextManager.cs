using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextManager : Singleton<UITextManager>
{

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesLeftText;

	// This is the overlay that tells the player that the game is over
	public GameObject GameOverPanel;

	public void Start()
	{
		GameOverPanel.SetActive(false);
	}

	public void SetWave(int wave)
    {
        waveText.text = "Wave " + wave + " of 10";
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
